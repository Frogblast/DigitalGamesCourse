using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;
    [Space]
    [SerializeField] private CameraSpring cameraSpring;
   


    private PlayerInputActions _inputActions;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

        playerCharacter.Initialize();
        playerCamera.Initialize(playerCharacter.GetCameraTarget());

        cameraSpring.Initialize();
    }

    private void OnDestroy()
    {
        _inputActions.Dispose();
    }

    void Update()
    {
        var input = _inputActions.Gameplay;
        var deltaTime = Time.deltaTime;

        // get camera pos and update rot
        var cameraInput = new CameraInput { Look = input.Look.ReadValue<Vector2>() };
        playerCamera.UpdateRotation(cameraInput);

        // get character input and update
        var characterInput = new CharacterInput
        {
            Rotation    = playerCamera.transform.rotation,
            Move        = input.Move.ReadValue<Vector2>(),
            Jump        = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Crouch      = input.Crouch.WasPressedThisFrame()
                ? CrouchInput.Toogle
                : CrouchInput.None,
            Interact    = input.Interact.WasPressedThisFrame()
        };
        playerCharacter.UpdateInput(characterInput);
        playerCharacter.UpdateBody(deltaTime);
    }

    private void LateUpdate()
    {
        var deltaTime = Time.deltaTime;
        var cameraTarget = playerCharacter.GetCameraTarget();

        playerCamera.UpdatePosition(cameraTarget);
        cameraSpring.UpdateSpring(deltaTime, cameraTarget.up);
    }
}
