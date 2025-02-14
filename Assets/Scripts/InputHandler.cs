using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerPhysics player;
    private CameraAiming cameraAiming;
    private Inventory inventory;
    private PlayerAudio playerAudio;

    [Header("Settings")]
    [SerializeField]
    private float mouseSensitivity = 50f;

    [Header("Components")]
    [SerializeField]
    private Camera mainCamera;


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
        player = GetComponent<PlayerPhysics>();
        inventory = GetComponent<Inventory>();
        playerAudio = GetComponentInChildren<PlayerAudio>();
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
        if (newVelocity.sqrMagnitude >= 0)
        {
            playerAudio.PlayWalkSound();
        }
    }

    public void OnPickUp(InputValue value)
    {
        if (inventory != null)
            inventory.TryPickUpItem();
    }

    public void OnDrop(InputValue value)
    {
        if (inventory != null)
        {
            Vector3 dropOffPosition = transform.position + mainCamera.transform.forward * 1.2f; // Spawn the item a bit in front of the player
            dropOffPosition.y = transform.position.y; // make sure the item doesn't spawn under floor level
            inventory.DropItem(dropOffPosition); 
        }
    }

    public void OnJump(InputValue value)
    {
        player.Jump(value);
        playerAudio.PlayJumpSound();
    }

    public void OnHotbar_1(InputValue value)
    {
        hotbarSelected = 0;
        hotbarChangeItem();
    }

    public void OnHotbar_2(InputValue value)
    {
        hotbarSelected = 1;
        hotbarChangeItem();
    }

    public void OnHotbar_3(InputValue value)
    {
        hotbarSelected = 2;
        hotbarChangeItem();
    }

    public void OnHotbar_4(InputValue value)
    {
        hotbarSelected = 3;
        hotbarChangeItem();
    }

    public void OnHotbar_5(InputValue value)
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
                scale = new Vector3(0.9f, 0.9f, 0.9f);
            }
            slot.transform.localScale = scale;
        }

    }

    
    


    public void OnDropItem(InputValue value) {
        Debug.Log("You dropped an item visually");

        hud.selectedSlot = hotbarSelected;
        inventoryscript.hotbarSelected = hotbarSelected;


        inventoryscript.DropItem(hotbarSelected);

        
       

    }

}
