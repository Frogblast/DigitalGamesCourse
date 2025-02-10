using System;
using UnityEngine;

public class CameraAnimationHandler : MonoBehaviour
{
    Animator animator;

    public enum State
    {
        Idling,
        Walking,
        Airborne
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationState(State state)
    {
        foreach (State s in Enum.GetValues(typeof(State)))
        {
            animator.SetBool("Is" + s.ToString(), false);
        }

        animator.SetBool("Is" + state.ToString(), true);
    }
}

