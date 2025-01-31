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
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit lethal zone");
            EventManager.TriggerPlayerDeath(); // Invoke the eventmanagers global event
        }
    }
}
