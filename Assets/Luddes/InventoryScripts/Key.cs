using UnityEngine;

public class Key : MonoBehaviour, IInventoryItem
{
    [SerializeField] private float offset = 2f; 
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
        gameObject.SetActive(false); // Disables the object, but keeps it in the scene
    }

    // Drops the object where the player is looking
    public void OnDrop()
    {
        GameObject camera = GameObject.Find("Main Camera");
        Vector3 dropPosition = camera.transform.position + camera.transform.forward * offset;
        float playerposition_y = camera.transform.position.y;
        if (dropPosition.y < 0.5) // If player is looking at the ground while dropping, this makes sure the object spawns on the ground
        {
            dropPosition = new Vector3(dropPosition.x, 1f, dropPosition.z);
        }
        transform.position = dropPosition;
        gameObject.SetActive(true);
    }
}
