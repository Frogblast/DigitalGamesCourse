using KinematicCharacterController;
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
        else if(other.gameObject.CompareTag("Character"))
        {
            Vector3 spawnpoint = new Vector3(0, 2, 0);

            // Don't know how to reset velocity to zero, this is to remove the accumulated velocity
            // from falling down to the OutOfBoundsTrigger.
            //other.GetComponent<KinematicCharacterMotor>().
            other.GetComponent<KinematicCharacterMotor>().SetPosition(spawnpoint);
        }
    }
}
