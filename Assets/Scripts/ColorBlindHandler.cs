using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public class ColorBlindHandler : MonoBehaviour
{
    Colorblind colorblindComponent;

    private void Start()
    {
        colorblindComponent = GetComponent<Colorblind>();
        colorblindComponent.enabled = false;
    }

    public void EnableColorBlindMode()
    {
        colorblindComponent.enabled = true;
    }

    int type = 0;
    public void ChangeMode()
    {
        colorblindComponent.Type = type;
        type++;
        if (type == 4) type = 0;
    }
}
