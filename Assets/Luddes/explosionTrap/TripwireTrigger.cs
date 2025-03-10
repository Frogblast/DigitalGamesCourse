using UnityEngine;
using UnityEngine.Events;

public class TripwireTrigger : MonoBehaviour
{

    public UnityEvent EnteredTrigger;
    private bool hasTriggered = false; // Only allows the bomb to go off once


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Character" && !hasTriggered) // "If triggered by player and has not been triggered before"
        {
            EnteredTrigger?.Invoke(); // Calls the event and everything subscribed to this event
            hasTriggered = true; // This now prevents the trigger from working again
        }
    }
}
