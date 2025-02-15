using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracThisBlock : MonoBehaviour, Interactable
{
    private Renderer _renderer;
    private Color originalColor;
    private bool isInteracting = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            originalColor = _renderer.material.color;
        }
    }
  
    public void Interact()
    {
        Debug.Log("You clicked a block");
       
        // toggle interaction state
        isInteracting = !isInteracting;

        // if is interacting, red, if not yellow
        _renderer.material.color = isInteracting ? Color.red : originalColor;
    }

    public void IsLookedAt(bool isLookedAt)
    {
        Debug.Log(isLookedAt + "This is a block in front of you");
        if (_renderer != null && !isInteracting)
        {
            _renderer.material.color = isLookedAt ? Color.yellow : originalColor;
        }
    }

}
