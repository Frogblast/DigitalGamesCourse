using System;
using UnityEngine;

public interface IInventoryItem
{

    Quaternion spawnrotation { get; }
    Vector3 spawnposition { get; }
    string Name { get; }
    Sprite Image { get; }

    void OnPickup();

    void OnDrop();
}

public class InventoryEventArgs : EventArgs
{
    public InventoryEventArgs(IInventoryItem item)
    {
        Item = item;
    }
    public IInventoryItem Item;
}



public class InventoryItem : MonoBehaviour
{

}
