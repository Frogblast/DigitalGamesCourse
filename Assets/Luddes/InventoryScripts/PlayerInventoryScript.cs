using UnityEngine;
using UnityEngine.InputSystem;


// This script is the only script that needs to be on a player
public class PlayerInventoryScript : MonoBehaviour
{


    private int hotbarSelected = 0;
    [SerializeField] private GameObject[] hotbarslots = new GameObject[5];

    [SerializeField] private InventoryScript inventoryscript;
    [SerializeField] private HUD hud;


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

        foreach (GameObject slot in hotbarslots)
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


    private void OnTriggerEnter(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventoryscript.AddItem(item);
        }
    }



}
