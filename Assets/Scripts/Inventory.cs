using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * The interface is TryPickUpItem() and DropItem(Vector3 dropPosition)
 * The "genericItemPrefab" is assigned in the inspector in unity. Now it is set to the treasure gameobject
 * but it would be good to be able to pick up any items, not just the treasure. This could be done by extending the Item scriptable object
 * so that it holds info on all of the item's components. That way the genericItemPrefab could be a simple empty gameobject which programatically
 * could be built with new components, where the recipes are the Item within the inventory list. I.e. factory pattern.
 */

public class Inventory : MonoBehaviour
{
    private List<Item> inventory = new List<Item>();
    private ItemBridge currentInteractableItem = null;

    [Header("Generic item to be dropped")]
    [SerializeField] private GameObject genericItemPrefab;

    public void TryPickUpItem()
    {
        if (currentInteractableItem != null)
        {
            inventory.Add(currentInteractableItem.data);

            Debug.Log($"Picked up {currentInteractableItem.data.ToString()}. Inventory now contains {inventory.Count} items.");

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
        int selectionIndex = 0;
        Debug.Log(inventory.Count);
        if (inventory.Count == 0)
        {
            Debug.Log("No item to drop.");
            return;
        }

        GameObject droppedItem = Instantiate(genericItemPrefab, dropPosition, Quaternion.identity);

        ItemBridge newItem = droppedItem.GetComponent<ItemBridge>();
        if (newItem == null)
        {
            newItem = droppedItem.AddComponent<ItemBridge>();
        }

        // Specify/restore original item's components
        newItem.data = inventory[selectionIndex];
        newItem.tag = "Treasure";
        newItem.GetComponent<BoxCollider>().isTrigger = false;

        inventory.RemoveAt(selectionIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemBridge>(out var item))
        {
            currentInteractableItem = item;
            Debug.Log($"Item in range: {item.data.itemName}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ItemBridge>(out var item) && currentInteractableItem == item)
        {
            currentInteractableItem = null;
            Debug.Log("Item out of range.");
        }
    }
}
