using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPathHint : MonoBehaviour
{

    public GameObject hint;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Character")
        { 
            Invoke("showHint", 20f); // shows the hint after 20 seconds of entering the invisible trap
        }
    }

    private void showHint()
    {
        hint.SetActive(true);
    }
    
}
