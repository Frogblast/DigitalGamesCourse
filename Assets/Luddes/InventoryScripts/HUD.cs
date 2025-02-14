using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public InventoryScript Inventory;
    internal int selectedSlot;

    private void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += InventoryScript_ItemRemoved;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        foreach (Transform slot in transform)
        {
            // Border... Image
            Image image = slot.GetChild(0).GetComponent<Image>();

            // we find first empty slot in UI
            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;

                break;
            }
        }
    }


    
    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform slot = transform.GetChild(selectedSlot);
        Image image = slot.GetChild(0).GetComponent<Image>();

                image.enabled = false;
                image.sprite = null;
    }




}

