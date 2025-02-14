using UnityEngine;

public class Key : MonoBehaviour, IInventoryItem
{
    public string Name
    {
        get
        {
            return "Key";
        }
    }


    public Sprite _Image = null;
    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public void OnPickup()
    {
        // Logic for what happens when picking up object. Ex. adding it to the players hand
        // For now, just remove the object
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        gameObject.SetActive(true);
    }
}
