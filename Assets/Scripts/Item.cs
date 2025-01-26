using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    public void PickUp()
    {
        Debug.Log($"Picked up {data.itemName}");
    }
}
