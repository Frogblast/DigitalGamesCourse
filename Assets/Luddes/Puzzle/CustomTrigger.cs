using UnityEngine;
using UnityEngine.Events;

public class CustomTrigger : MonoBehaviour
{

    public UnityEvent<Collider> EnteredTrigger;


    private void OnTriggerEnter(Collider collider)
    {
        EnteredTrigger?.Invoke(collider);
    }

}
