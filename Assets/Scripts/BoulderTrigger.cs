using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject boulder;
    public AudioSource boulderAudio;
    public bool audioPlayed = false;
    private bool hastriggered = false;

    private void Start()
    {
        if (boulder != null)
        {
            boulderAudio = boulder.GetComponentInChildren<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Character"))
        {
            if (!hastriggered)
            {
                hastriggered=true;
                MeshCollider meshCollider = targetObject.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.enabled = false;
                }
                if (!audioPlayed && boulderAudio != null)
                {
                    boulderAudio.Play();
                    audioPlayed = true;
                }
            }
        }
    }
}
