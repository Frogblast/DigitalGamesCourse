using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisiblityToggler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Character"))
        {

        //Debug.Log("collided");
        this.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
