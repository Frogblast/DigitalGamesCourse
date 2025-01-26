using UnityEngine;

/*
 * This class acts as a bridge between the gameobject in unity and the "Item" scriptable object (since unity doesn't allow scriptable objects to
 * be added directly as components to gameobjects).
 */

public class ItemBridge : MonoBehaviour
{
    public Item data;

    public void PickUp()
    {
        Debug.Log($"Picked up {data.itemName}");
    }
}
