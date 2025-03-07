using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;
   // [SerializeField] private FelixInventory playerInventory;
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
        // initalise playerInventory

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
            Rotation = playerCamera.transform.rotation,
            Move = input.Move.ReadValue<Vector2>(),
            Jump = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Crouch = input.Crouch.WasPressedThisFrame()
                ? CrouchInput.Toogle
                : CrouchInput.None,
            Interact = input.Interact.WasPressedThisFrame()
        };
        playerCharacter.UpdateInput(characterInput);
        playerCharacter.UpdateBody(deltaTime);

        // this is stupid (vad betyder den här kommentaren???)
        int selectedSlot = -1;
        if (input.SelectSlot.WasPerformedThisFrame())
        {
            selectedSlot = GetSelectedInvSlot();
        }

        var inventoryInput = new InventoryInput
        {
            Drop = input.Drop.WasPerformedThisFrame(),
            SelectedSlot = selectedSlot
        };

       // playerInventory.HandleInput(inventoryInput);

        // send the input of inventory to playerInv.Updatesomething
    }

    private void LateUpdate()
    {
        var deltaTime = Time.deltaTime;
        var cameraTarget = playerCharacter.GetCameraTarget();

        playerCamera.UpdatePosition(cameraTarget);
        cameraSpring.UpdateSpring(deltaTime, cameraTarget.up);
    }

    private int GetSelectedInvSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 2;
        return -1;
    }

    public void ToggleColorBlindMode(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;
        ColorBlindHandler handler = transform.GetComponentInChildren<ColorBlindHandler>();
        handler.EnableColorBlindMode();
        handler.ChangeMode();
    }

    public void Walk(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerCharacter.walking = true;
        }
        else if(context.canceled)
        {
            playerCharacter.walking = false;
        }
    }
}
