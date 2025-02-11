using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public InventoryScript Inventory;

    private void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        foreach (Transform slot in transform)
        {
            // Border... Image
            Image image = slot.GetChild(0).GetComponent<Image>();
            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;

                // TODO: Store a reference to the item

                break;
            }
        }
    }



}
