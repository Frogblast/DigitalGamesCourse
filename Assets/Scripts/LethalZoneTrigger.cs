using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LethalZoneTrigger : MonoBehaviour
{
    private Collider _collider;

    public event Action OnLethalZoneEnter;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit lethal zone");
            OnLethalZoneEnter?.Invoke(); // Only invoke if there is atleast one subscriber
        }
    }
}
