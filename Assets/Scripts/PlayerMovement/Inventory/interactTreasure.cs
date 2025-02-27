using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactTreasure : MonoBehaviour, Interactable
{
    private Renderer _renderer;
    private Color originalColor;
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
        Debug.Log("You win!");
        EventManager.TriggerPlayerDeath();
    }

    public void IsLookedAt(bool isLookedAt)
    {
        Debug.Log(isLookedAt + "This is a treasure in front of you");
        if (_renderer != null)
        {
            _renderer.material.color = isLookedAt ? Color.yellow : originalColor;
        }
    }

  

   
}
