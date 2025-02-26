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
        gameObject.SetActive(false); // Disables the object, but keeps it in the scene
    }

    // Drops the object where the player is looking
    public void OnDrop()
    {
        GameObject camera = GameObject.Find("Camera");
        /*Vector3 dropPosition = camera.transform.position + camera.transform.forward * offset;
        if (dropPosition.y < 0.5f) // If player is looking at the ground while dropping, this makes sure the object spawns on the ground
        {
            dropPosition = new Vector3(dropPosition.x, 1f, dropPosition.z);
        }
        transform.position = dropPosition;
        gameObject.SetActive(true);*/

        Vector3 rpos = camera.transform.position;
        Vector3 rdir = camera.transform.forward;
        float distance = 3f;
        Ray ray = new Ray(rpos, rdir);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, distance))
        {
            transform.position = hit.point + new Vector3(0, 0.5f, 0);
            gameObject.SetActive(true);
        }
        else
        {
            Vector3 dropPosition = ray.GetPoint(distance);
            transform.position = dropPosition;
            gameObject.SetActive(true);
        }
    }
}
