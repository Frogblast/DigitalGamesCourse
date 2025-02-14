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

    public void AddItem(IInventoryItem item)
    {
        bool canPickup = false;
        int i = 0;
        for (i = 0; i < SLOTS; i++)
        {
            if (mItems[i] == null){
                canPickup = true;
                break;
            }
        }
        //if (mItems.Length < SLOTS)
        if (canPickup)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                //mItems.Add(item);
                mItems[i] = item;

                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }

            }
        }
    }

    public void DropItem(int hotbarSelected)
    {
        IInventoryItem item = mItems[hotbarSelected];
        //mItems.Remove(item);
        mItems[hotbarSelected] = null;
        Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
        collider.enabled = true;

        item.OnDrop();


        if(ItemRemoved != null)
        {
            ItemRemoved(this, new InventoryEventArgs(item));
        }
    }
}
