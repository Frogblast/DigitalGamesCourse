using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryScript : MonoBehaviour
{
    private const int SLOTS = 5;
    internal int hotbarSelected = 0;

    internal IInventoryItem[] mItems = new IInventoryItem[SLOTS];

    public event EventHandler<InventoryEventArgs> ItemAdded;

    public event EventHandler<InventoryEventArgs> ItemRemoved;


    internal bool slotIsEmpty(int hotbarSelected) // Checks if the inventorylist is empty at specific index
    {
        return mItems[hotbarSelected] == null;    

    }



    public void AddItem(IInventoryItem item)
    {
        bool canPickup = false;
        int i = 0;
        for (i = 0; i < SLOTS; i++) // Checks if there is an avaliable slot
        {
            if (mItems[i] == null){
                canPickup = true;
                break;
            }
        }

        if (canPickup) // If slot avaliable, then add item
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled) // If collider is true, then set it to false while in inventory
            {
                collider.enabled = false;
                mItems[i] = item; // Add item to the list

                item.OnPickup(); // Calls the logic for what happens when picking up this specific item

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }

            }
        }
    }

    public void DropItem(int hotbarSelected)
    {
        IInventoryItem item = mItems[hotbarSelected]; // Select what item from the list to drop
        mItems[hotbarSelected] = null; // Remove the item from the list
        Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
        collider.enabled = true; // Enable the collider again before dropping it

        item.OnDrop(); // Calls the logic for what happens when dropping this specific item


        if(ItemRemoved != null)
        {
            ItemRemoved(this, new InventoryEventArgs(item));
        }
    }
}
