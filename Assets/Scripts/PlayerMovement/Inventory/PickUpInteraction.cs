using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickUpInteraction : MonoBehaviour, Interactable
{
    private Renderer _renderer;
    private Color originalColor;
    private bool isInteracting = false;

    public ItemData itemData; 

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
        Debug.Log("You clicked a pickup object " + itemData.itemName);

        FelixInventory.Instance.AddItem(itemData);
        
        // initalise the hotbar focus effect 

        Destroy(gameObject);

    }

    public void IsLookedAt(bool isLookedAt)
    {
        //  Debug.Log(isLookedAt + "This is a pickup object in front of you");
        if (_renderer != null && !isInteracting)
        {
            _renderer.material.color = isLookedAt ? Color.green : originalColor;
        }
    }
}
