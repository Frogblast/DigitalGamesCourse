using UnityEngine;

public class OutOfBounds : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IInventoryItem>() != null)
        {
            other.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        }
    }
}
