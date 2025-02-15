using System.Data;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField]
    private InventoryScript inventoryscript;
    [SerializeField]
    private GameObject[] hotbarslots = new GameObject[5];
    
    private int hotbarSelected = 0;

    [SerializeField]
    private HUD hud;


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
        playerPhysics.Jump(context);
        playerAudio.PlayJumpSound();
    }

    public void OnHotbar_1(InputAction.CallbackContext context)
    {

        hotbarSelected = 0;
        hotbarChangeItem();
    }

    public void OnHotbar_2(InputAction.CallbackContext context)
    {
        hotbarSelected = 1;
        hotbarChangeItem();
    }

    public void OnHotbar_3(InputAction.CallbackContext context)
    {
        hotbarSelected = 2;
        hotbarChangeItem();
    }

    public void OnHotbar_4(InputAction.CallbackContext context)
    {
        hotbarSelected = 3;
        hotbarChangeItem();
    }

    public void OnHotbar_5(InputAction.CallbackContext context)
    {
        hotbarSelected = 4;
        hotbarChangeItem();
    }


    private void hotbarChangeItem()
    {
        inventoryscript.hotbarSelected = hotbarSelected; 

        foreach(GameObject slot in hotbarslots)
        {
            Vector3 scale;

            if (slot == hotbarslots[hotbarSelected])
            {
                scale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            else
            {
                scale = new Vector3(1f, 1f, 1f);
            }
            slot.transform.localScale = scale;
        }

    }




    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (context.started && !inventoryscript.slotIsEmpty(hotbarSelected)) 
        { 
            hud.selectedSlot = hotbarSelected;
            inventoryscript.hotbarSelected = hotbarSelected;

            inventoryscript.DropItem(hotbarSelected);
        }

    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerPhysics.IsSprinting = true;
        } 
        else if (context.canceled)
        {
            playerPhysics.IsSprinting = false;
        }
    }

    public void ToggleColorBlindMode(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;
        ColorBlindHandler handler = mainCamera.GetComponent<ColorBlindHandler>();
        handler.EnableColorBlindMode();
        handler.ChangeMode();
    }
}
