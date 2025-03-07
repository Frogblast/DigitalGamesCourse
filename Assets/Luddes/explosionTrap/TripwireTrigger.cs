using UnityEngine;
using UnityEngine.Events;

public class TripwireTrigger : MonoBehaviour
{

    public UnityEvent EnteredTrigger;
    private bool hasTriggered = false;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Character" && !hasTriggered)
        {
            EnteredTrigger?.Invoke();
            hasTriggered = true;
            Debug.Log("Triggereed");
        }
    }

}
