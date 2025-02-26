using UnityEngine;

public class GoldBar : MonoBehaviour, IInventoryItem
{
    [SerializeField] private float offset = 2f;

    public Sprite _Image;
    public string Name 
    { get 
        { 
            return "GoldBar"; 
        } 
    }

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public void OnDrop()
    {
        GameObject camera = GameObject.Find("Main Camera");
        Vector3 dropPosition = camera.transform.position + camera.transform.forward * offset;
        if (dropPosition.y < camera.transform.position.y - 1) // If player is looking at the ground while dropping, this makes sure the object spawns on the ground
        {
            dropPosition = new Vector3(dropPosition.x, camera.transform.position.y-1, dropPosition.z);
        }
        transform.position = dropPosition;
        gameObject.SetActive(true);
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }
}
