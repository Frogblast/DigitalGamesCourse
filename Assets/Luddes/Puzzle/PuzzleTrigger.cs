using UnityEngine;
using UnityEngine.Events;

public class PuzzleTrigger : MonoBehaviour
{

    public CustomTrigger keyTrigger;
    public CustomTrigger GoldBarTrigger;

    private bool isKeyPlaced;
    private bool isGoldbarPlaced;

    private void Awake()
    {
        keyTrigger.EnteredTrigger.AddListener(OnKeyTriggerEntered);
        GoldBarTrigger.EnteredTrigger.AddListener(OnGoldbarTriggerEntered);
    }


    void OnKeyTriggerEntered(Collider collider)
    {
        if (collider.tag == "Key")
        {
            isKeyPlaced = true;
        }
    }

    void OnGoldbarTriggerEntered(Collider collider)
    {
        if (collider.tag == "GoldBar")
        {
            isGoldbarPlaced = true;
        }
    }

    private void Update()
    {
        if (isKeyPlaced == true && isGoldbarPlaced == true)
        {
            gameObject.SetActive(false);
        }
    }



}
