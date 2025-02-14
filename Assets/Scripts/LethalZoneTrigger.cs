using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LethalZoneTrigger : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            Debug.Log("Player hit lethal zone");
            EventManager.TriggerPlayerDeath(); // Invoke the eventmanagers global event
        }
    }

    // for boulder collision, break this out in its own function if it breaks something else
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Debug.Log("Player collided with death");
            EventManager.TriggerPlayerDeath();
        }
    }
}
