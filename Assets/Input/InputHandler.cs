using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerPhysics player;
    private CameraAiming cameraAiming;
    [SerializeField]
    private float mouseSensitivity = 50f;
    [SerializeField]
    private Camera mainCamera;

    private Inventory inventory;


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
    }

    public void OnMovement(InputValue value)
    {
        Vector2 newVelocity = value.Get<Vector2>();

        player.ChangeVelocity(newVelocity);
    }
    public void OnPickUp(InputValue value)
    {
        if (inventory != null)
        {
            inventory.TryPickUpItem();
        }
    }

    public void OnDrop(InputValue value)
    {
        if (inventory != null)
        {
            inventory.DropItem(transform.position);
        }
    }
}
