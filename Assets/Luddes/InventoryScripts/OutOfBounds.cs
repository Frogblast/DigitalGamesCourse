using UnityEngine;

public class OutOfBounds : MonoBehaviour
{

    // If any specified object enters this trigger, it will be respawned in someplace of the level
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IInventoryItem>() != null) // Checks if object is a pickup item
        {
            Vector3 respawn_position = other.GetComponent<IInventoryItem>().spawnposition; // Gets the original spawn position of the object
            Quaternion respawn_rotation = other.GetComponent<IInventoryItem>().spawnrotation;
            other.gameObject.transform.position = respawn_position; // Applys position
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; // resets it's speed to 0 (it accumulates speed as it falls out of bounds)
            other.gameObject.transform.rotation = respawn_rotation;
        }
    }
}
