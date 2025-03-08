using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPathHint : MonoBehaviour
{

    [SerializeField] private int hint1_delay = 20;
    [SerializeField] private int hint2_delay = 50;

    public GameObject hint;
    public GameObject hint_2;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Character")
        { 
            Invoke("showHint", hint1_delay); // shows the hint after 20 seconds of entering the invisible trap

            Invoke("showHint_nr2", hint2_delay); // displays the second hint after 40 seconds of entering the invisible trap
        }
    }

    private void showHint()
    {
        hint.SetActive(true);
    }

    private void showHint_nr2()
    {
        hint_2.SetActive(true);
    }
    
}
