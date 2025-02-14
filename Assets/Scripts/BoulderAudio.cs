using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderAudio : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boulder"))
        {
            AudioSource boulderAudio = other.GetComponentInChildren<AudioSource>();
            if (boulderAudio != null )
            {
                boulderAudio.Stop();
            }
        }
    }
}
