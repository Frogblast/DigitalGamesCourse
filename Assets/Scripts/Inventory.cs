using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used by 
 * Interface is TryPickUpItem() and DropItem(Vector3 dropPosition) (dropPosition is passed from PlayerControls)
 */

public class Inventory : MonoBehaviour
{
    private List<ItemData> inventory = new List<ItemData>();
    private Item currentInteractableItem = null;

    [SerializeField] private GameObject genericItemPrefab;

    public void TryPickUpItem()
    {
        if (currentInteractableItem != null)
        {
            inventory.Add(currentInteractableItem.data);

            Debug.Log($"Picked up {currentInteractableItem.data.itemName}. Inventory now contains {inventory.Count} items.");

            Destroy(currentInteractableItem.gameObject);
            currentInteractableItem = null;
        }
        else
        {
            Debug.Log("No item to pick up.");
        }
    }

    public void DropItem(Vector3 dropPosition)
    {
        if (inventory.Count == 0)
        {
            Debug.Log("No item to drop.");
            return;
        }

        GameObject droppedItem = Instantiate(genericItemPrefab, dropPosition, Quaternion.identity);
        Item newItem = droppedItem.GetComponent<Item>();
        newItem.data = inventory[0];

        inventory.RemoveAt(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Item>(out var item))
        {
            currentInteractableItem = item;
            Debug.Log($"Item in range: {item.data.itemName}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Item>(out var item) && currentInteractableItem == item)
        {
            currentInteractableItem = null;
            Debug.Log("Item out of range.");
        }
    }
}
