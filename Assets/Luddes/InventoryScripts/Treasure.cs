using UnityEngine;

public class Treasure : MonoBehaviour, IInventoryItem
{
    public string Name
    {
        get
        {
            return "Treasure";
        }
    }

    public Sprite _Image;
    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        gameObject.SetActive(true);
    }
}
