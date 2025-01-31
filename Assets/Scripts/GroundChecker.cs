using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded {  get; private set; } = true;

    private void OnTriggerEnter()
    {
        IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsGrounded = false;
    }
}
