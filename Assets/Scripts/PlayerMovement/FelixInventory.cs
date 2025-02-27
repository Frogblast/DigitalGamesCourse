using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public struct InventoryInput
{
    public bool Drop;
    public int SelectedSlot;
}

public class FelixInventory : MonoBehaviour
{
    public static FelixInventory Instance;
    public List<ItemData> inventory = new List<ItemData>();
    
    public Transform toolbarUI;
    public GameObject inventorySlotPrefab;

    private int selectedSlotIndex = 0;
    private List<GameObject> slotObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        Debug.Log("AddItem:" + item.itemName);
        inventory.Add(item);
        UpdateToolbarUI();
    }

    public void HandleInput(InventoryInput input)
    {
        // If number pressed select that hotbar slot
        if (input.SelectedSlot != -1)
        {
            SelectSlot(input.SelectedSlot);
        }

        // Handle item dropping
        if (input.Drop)
        {
            DropSelectedItem();
        }
    }

    private void SelectSlot(int selectedSlot)
    {
        Debug.Log("Selected Slot: " + selectedSlot);
        selectedSlotIndex = selectedSlot;
        UpdateToolbarUI();
    }

    private void DropSelectedItem()
    {
        if (inventory.Count == 0) return;
        if (selectedSlotIndex >= inventory.Count) return; 

        ItemData itemToDrop = inventory[selectedSlotIndex];

        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f; // drop 2f infront
        Quaternion spawnRotation = Quaternion.identity; // no rotation, just for the instantiate 
        GameObject droppedItem = Instantiate(itemToDrop.itemPrefab, spawnPosition, spawnRotation);


        inventory.RemoveAt(selectedSlotIndex);
        Debug.Log("item removed index: " + selectedSlotIndex);

        UpdateToolbarUI();
        Debug.Log("Dropped: " + itemToDrop.itemName);
    }

    private void UpdateToolbarUI()
    {
        // since we loop through the already filled inventory array the old slots need to be deleted before adding them all again
        foreach (Transform child in toolbarUI)
        {
            Destroy(child.gameObject);
        }
        slotObjects.Clear();

        if (inventory.Count == 0)
        {
            Debug.Log("Inventory is empty");
            return;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, toolbarUI);
            slotObjects.Add(slot);
            Debug.Log("Successful inisitate");

            Image itemIcon = slot.transform.GetChild(1).GetComponent<Image>();
            Debug.Log("ItemIcon found: " + itemIcon);

            itemIcon.sprite = inventory[i].icon;
            itemIcon.enabled = true;

            Image slotBackground = slot.transform.GetChild(0).GetComponent<Image>(); // Assuming the first child is the background
            slotBackground.color = (i == selectedSlotIndex) ? Color.yellow : Color.white;


        }
    }

}
