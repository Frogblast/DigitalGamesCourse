using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float tapJumpForce = 5f;
    [SerializeField] private float groundDetectionDistance = 1.3f;

    private Vector2 velocity = Vector2.zero;
    private Rigidbody rb;
    private bool isAlive = true;
    private CameraAnimationHandler CameraAnimationHandler;

    public Vector3 LocalSpace { get; set; } = Vector3.zero;
    public bool IsSprinting { get; internal set; } = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CameraAnimationHandler = GetComponentInChildren<CameraAnimationHandler>();
    }

    private bool IsGrounded()
    { 
        return Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance); ;
    }

    private void OnEnable()
    {
        EventManager.OnPlayerDeath += KillPlayer;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= KillPlayer;
    }

    private void KillPlayer()
    {
        if (!isAlive) return;
        isAlive = false;
    }

    private void Update()
    {
        ApplyMovement();
        ApplyCameraAnimation();
    }

    private void ApplyCameraAnimation()
    {
        if (Mathf.Abs(rb.velocity.y) > float.Epsilon)
        {
            CameraAnimationHandler.SetAnimationState(CameraAnimationHandler.State.Airborne);
            return;
        }

        if (Mathf.Abs(rb.velocity.x) > float.Epsilon || Mathf.Abs(rb.velocity.z) > float.Epsilon)
        {
            CameraAnimationHandler.SetAnimationState(CameraAnimationHandler.State.Walking);
            return;
        }

        CameraAnimationHandler.SetAnimationState(CameraAnimationHandler.State.Idling);
    }

    private void ApplyMovement()
    {
        if (LocalSpace == Vector3.zero) return; // Don't apply movement if there is no updated direction
     
        float currentSpeed = IsSprinting? sprintSpeed : walkSpeed; // Set the actual speed to be applied according to the IsSprinting bool

        Vector3 forward = LocalSpace;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * velocity.y + right * velocity.x;

        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    internal void ChangeVelocity(Vector2 vector2)
    {
        velocity = vector2;
    }

    internal void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        bool tapped = context.interaction is UnityEngine.InputSystem.Interactions.TapInteraction;

        if (context.performed && tapped) // If tapping - cut the velocity in y direction
        {
            float tapJumpMultiplier = 0.6f;
            if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * tapJumpMultiplier, rb.velocity.z);
        }
    }
}
