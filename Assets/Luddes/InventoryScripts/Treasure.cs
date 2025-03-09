using UnityEngine;

public class Treasure : MonoBehaviour, IInventoryItem
{
    public Vector3 spwn_position;
    private Quaternion spwn_rotation;

    private void Start()
    {
        spwn_position = transform.position;
        spwn_rotation = transform.rotation;
    }

    public Quaternion spawnrotation
    {
        get
        {
            return spwn_rotation;
        }
    }


    public Vector3 spawnposition
    {
        get
        {
            return spwn_position;
        }
    }


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
        gameObject.SetActive(false); // Disables the object, but keeps it in the scene
    }

    // Drops the object where the player is looking
    public void OnDrop()
    {
        GameObject camera = GameObject.Find("Camera");
       
        Vector3 rpos = camera.transform.position;
        Vector3 rdir = camera.transform.forward;
        float distance = 3f;
        Ray ray = new Ray(rpos, rdir); // Spawn a ray with the same position and direction as the playercamera
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, distance)) // If we hit something with the ray (mainly floors), spawn a bit further up to prevent collision.
        {
            transform.position = hit.point + new Vector3(0, 0.5f, 0); // Place item a bit above the floor where we are looking
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
