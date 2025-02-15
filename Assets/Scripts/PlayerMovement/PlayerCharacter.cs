using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public enum CrouchInput
{  // add hold to crouch
    None, Toogle
}
public enum Stance
{
    Stand, Crouch
}

public struct CharacterInput 
{
    public Quaternion Rotation;
    public Vector2 Move;
    public bool Jump;
    public bool JumpSustain;
    public CrouchInput Crouch;
    public bool Interact;
}

public class PlayerCharacter : MonoBehaviour, ICharacterController
{
   
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private Transform root;
    [SerializeField] private Transform cameraTarget;
    [Space]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float walkResponse = 25f;
    [SerializeField] private float crouchResponse = 20f;
    [Space]
    [SerializeField] private float airSpeed = 15f;
    [SerializeField] private float airAcceleration = 70;
    [Space]
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float coyoteTime = 0.2f;
    [Range(0f, 1f)]
    [SerializeField] private float jumpSustainGravity = 0.4f; 
    [SerializeField] private float gravity = -90f;
    [Space]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchHeightResponse = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float standCameraTargetHeight = 0.9f;
    [Range(0f, 1f)]
    [SerializeField] private float crouchCameraTargetHeight = 0.7f;
    [Space]
    [SerializeField] private PlayerAudio playerAudio;

    private Stance _stance;

    private Quaternion _requestedRotation;
    private Vector3 _requestedMovement;
    private bool _requestedJump;
    private bool _requestedSustainedJump;
    private bool _requestedCrouch;

    private float _timeSinceUngrounded;
    private float _timeSinceJumpRequest;
    private bool _ungroundedDueToJump;

    private Collider[] _uncrouchOverlapResults;

    public void Initialize()
    {
        _stance = Stance.Stand;
        _uncrouchOverlapResults = new Collider[8];

        motor.CharacterController = this;
    }

    public void UpdateInput(CharacterInput input)
    {
        _requestedRotation = input.Rotation;
        _requestedMovement = new Vector3(input.Move.x, 0f, input.Move.y);
        _requestedMovement = Vector3.ClampMagnitude(_requestedMovement, 1f);
        _requestedMovement = input.Rotation * _requestedMovement;
        
        CastRay(input);
   
        var wasRequestingJump = _requestedJump;
        _requestedJump = _requestedJump || input.Jump;
        if (_requestedJump && !wasRequestingJump)
            playerAudio.PlayJumpSound(); // bandaid audio for jumping needs polish later
            _timeSinceJumpRequest = 0f;

        _requestedSustainedJump = input.JumpSustain;

        _requestedCrouch = input.Crouch switch
        {
            CrouchInput.Toogle => !_requestedCrouch,
            CrouchInput.None => _requestedCrouch,
            _ => _requestedCrouch
        };
    }

    // For dealing with crouching
    public void UpdateBody(float deltaTime)
    {
        var currentHeight = motor.Capsule.height;
        var normalizedHeight = currentHeight / standHeight;
        var cameraTargetHeight = currentHeight *
        (
            _stance is Stance.Stand
                ? standCameraTargetHeight
                : crouchCameraTargetHeight
        );
        var rootTargetScale = new Vector3(1f, normalizedHeight, 1f);

        cameraTarget.localPosition = Vector3.Lerp
            (
                a: cameraTarget.localPosition,
                b: new Vector3(0f, cameraTargetHeight, 0f),
                t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
            );

        root.localScale = Vector3.Lerp
        (
            a: root.localScale,
            b: rootTargetScale,
            t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
        );
    }

    // Acceleration both on ground and in the air and differentiate between walk and crouch speed
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // if on ground 
        if (motor.GroundingStatus.IsStableOnGround)
        {
            _timeSinceUngrounded = 0f;
            _ungroundedDueToJump = false;

            var groundedMovement = motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;

            var speed = _stance is Stance.Stand
                ? walkSpeed
                : crouchSpeed;
            var response = _stance is Stance.Stand
                ? walkResponse
                : crouchResponse; 
            // smooth acceleration on movement
            var targetVelocity = groundedMovement * speed;

            if (targetVelocity.magnitude > 0f) // bandaid audio for walking
                playerAudio.PlayWalkSound();

            currentVelocity = Vector3.Lerp
            (
                a: currentVelocity,
                b: targetVelocity,
                t: 1f - Mathf.Exp(-response * deltaTime)
            );
        }
        // else in the air
        else
        {
            _timeSinceUngrounded += deltaTime;
            // In-air movement
            if (_requestedMovement.sqrMagnitude > 0f)
            {
                var planarMovement = Vector3.ProjectOnPlane
                (
                    vector: _requestedMovement,
                    planeNormal: motor.CharacterUp
                ) * _requestedMovement.magnitude; // normalize

                var currentPlanarVelocity = Vector3.ProjectOnPlane
                (
                    vector: currentVelocity,
                    planeNormal: motor.CharacterUp
                );

                var movementForce = planarMovement * airAcceleration * deltaTime;
                var targetPlanarVelocity = currentPlanarVelocity + movementForce;
                targetPlanarVelocity = Vector3.ClampMagnitude(targetPlanarVelocity, airSpeed);
                currentVelocity += targetPlanarVelocity - currentPlanarVelocity;
            }

            // Gravity
            var effectiveGravity = gravity;
            var verticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            if (_requestedSustainedJump && verticalSpeed > 0f)
                effectiveGravity *= jumpSustainGravity;
            currentVelocity += motor.CharacterUp * effectiveGravity * deltaTime;
        }

        if (_requestedJump)
        {
            var grounded = motor.GroundingStatus.IsStableOnGround;
            var canCoyoteJump = _timeSinceUngrounded < coyoteTime && !_ungroundedDueToJump;

            if ( grounded || canCoyoteJump ) 
            {
                _requestedJump = false;

                motor.ForceUnground(time: 0f);
                _ungroundedDueToJump = true;

                var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
                var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, jumpSpeed);
                currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
            }
            else
            {
                _timeSinceJumpRequest += deltaTime;

                var canJumpLater = _timeSinceJumpRequest < coyoteTime;
                _requestedJump = canCoyoteJump;
            }
           
        }
    }
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

        var forward = Vector3.ProjectOnPlane
        (
            _requestedRotation * Vector3.forward,
            motor.CharacterUp
        );
        if (forward != Vector3.zero)
        {
            currentRotation = Quaternion.LookRotation(forward, motor.CharacterUp);
        } 
    }


    public void BeforeCharacterUpdate(float deltaTime) 
    { 
        // on crouch
        if (_requestedCrouch && _stance is Stance.Stand)
        {
            _stance = Stance.Crouch;
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: crouchHeight,
                yOffset: crouchHeight * 0.5f
            );
        }
    }
    
    // Checks if anything is above the playerPhysics to not noclip the head
    public void AfterCharacterUpdate(float deltaTime)
    {
        // uncrouch
        if (!_requestedCrouch && _stance is not Stance.Stand)
        {
            
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: standHeight,
                yOffset: standHeight * 0.5f
            );

            var pos = motor.TransientPosition;
            var rot = motor.TransientRotation;
            var mask = motor.CollidableLayers;

            if (motor.CharacterOverlap(pos, rot, _uncrouchOverlapResults, mask, QueryTriggerInteraction.Ignore) > 0)
            {
                _requestedCrouch = true;
                motor.SetCapsuleDimensions
                (
                    radius: motor.Capsule.radius,
                    height: crouchHeight,
                    yOffset: crouchHeight * 0.5f
                );
            }
            else
            {
                _stance = Stance.Stand;
            }
        }
    }

    // doubt if it should be here but it works, a bit ugly
    private Interactable lastInteractable = null;
    public void CastRay(CharacterInput input) // rename to lookray or something, is not performing interact anymore
    {
        // get main camera info
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;
        float rayDistance = 5f; // Interaction distance make serialized if I want later

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 0.1f);

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        Interactable currentInteractable = null;

        // send the ray
        if (Physics.Raycast(ray, out hit, 5f))
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();
            // the object with the interacable interface script on is hit with ray
            if (currentInteractable != null)
            {
                currentInteractable.IsLookedAt(true); // activate that the object is hovered over visibly in game
                if(input.Interact)
                {
                    // if object that is hovered over is interacted with (e is pressed)
                    Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.green, 5);
                    currentInteractable.Interact();
                }
            }
        }
        // if we stopped looking at an object, reset the cololr
        if (lastInteractable != currentInteractable)
        {
            lastInteractable?.IsLookedAt(false);
            lastInteractable = currentInteractable; // reset the color to nothing
        }
    }


    // interface functions not in use
    public bool IsColliderValidForCollisions(Collider coll) => true;

    public void OnDiscreteCollisionDetected(Collider hitCollider){}

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}

    public void PostGroundingUpdate(float deltaTime){}

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport){}

   
    // Refrence o the cameratarget where the camera clips too
    public Transform GetCameraTarget() => cameraTarget;
}
