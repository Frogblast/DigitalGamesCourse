using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("at drop off: " + other.gameObject);
        if (other.CompareTag("Treasure"))
        {
            Debug.Log("Treasure taken to drop-off point");
        }
    }
}
