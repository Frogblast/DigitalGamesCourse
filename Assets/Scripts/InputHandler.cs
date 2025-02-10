using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float mouseSensitivity = 50f;

    [Header("Components")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private CameraAiming cameraAiming;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private PlayerAudio playerAudio;

    private PlayerPhysics playerPhysics;

    private void Start()
    {
        cameraAiming = new CameraAiming(mainCamera, mouseSensitivity);
        Cursor.lockState = CursorLockMode.Locked;
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    private void Update()
    {
        cameraAiming.UpdateMousePosition();
        cameraAiming.Aim();
        playerPhysics.LocalSpace = mainCamera.transform.forward; // To update the playerPhysics local space to match the camera's
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 newVelocity = context.ReadValue<Vector2>();
        playerPhysics.ChangeVelocity(newVelocity);
        if (newVelocity.sqrMagnitude >= 0)
        {
            playerAudio.PlayWalkSound();
        }
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        if (inventory != null)
            inventory.TryPickUpItem();
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (inventory != null)
        {
            Vector3 dropOffOffset = mainCamera.transform.forward;
            dropOffOffset.Normalize();
            Vector3 dropOffPosition = transform.position + dropOffOffset * 3f; // Spawn the item a bit in front of the playerPhysics
            dropOffPosition.y = transform.position.y; // make sure the item doesn't spawn under floor level
            inventory.DropItem(dropOffPosition); 
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        playerPhysics.Jump();
        playerAudio.PlayJumpSound();
    }
}
