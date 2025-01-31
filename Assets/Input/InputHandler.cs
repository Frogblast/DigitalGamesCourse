using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerPhysics player;
    private CameraAiming cameraAiming;
    private Inventory inventory;

    [Header("Settings")]
    [SerializeField]
    private float mouseSensitivity = 50f;

    [Header("Components")]
    [SerializeField]
    private Camera mainCamera;


    private void Start()
    {
        cameraAiming = new CameraAiming(mainCamera, mouseSensitivity);
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<PlayerPhysics>();
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        cameraAiming.UpdateMousePosition();
        cameraAiming.Aim();
        player.LocalSpace = mainCamera.transform.forward; // To update the player local space to match the camera's
    }

    public void OnMovement(InputValue value)
    {
        Vector2 newVelocity = value.Get<Vector2>();
        player.ChangeVelocity(newVelocity);
    }

    public void OnPickUp(InputValue value)
    {
        if (inventory != null)
            inventory.TryPickUpItem();
    }

    public void OnDrop(InputValue value)
    {
        if (inventory != null)
            inventory.DropItem(transform.position);
    }

    public void OnJump(InputValue value)
    {
        player.Jump(value);
    }
}
