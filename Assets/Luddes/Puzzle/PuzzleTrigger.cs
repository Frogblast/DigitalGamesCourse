using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleTrigger : MonoBehaviour
{

    public CustomTrigger keyTrigger;
    public CustomTrigger GoldBarTrigger;

    public Camera mainCamera;
    public Camera secondaryCamera;
    public GameObject gate;

    public float gateRaiseHeight = 5f;
    public float animationDuration = 2f;

    private bool isKeyPlaced;
    private bool isGoldbarPlaced;

    private void Awake()
    {
        secondaryCamera.gameObject.SetActive(false);
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
            StartCoroutine(HandleUnlock());
            
        }
    }

    // little animation
    private IEnumerator HandleUnlock()
    {

        mainCamera.gameObject.SetActive(false); // turn off main camera
        secondaryCamera.gameObject.SetActive(true); // turn on alternative camera

        Vector3 targetPos = gate.transform.position + new Vector3 (0, gateRaiseHeight, 0); // variables to setup the gate going up function
        float elapsedTime = 0;
        Vector3 startPos = gate.transform.position;

        while (elapsedTime < animationDuration)
        { 
            gate.transform.position = Vector3.Lerp (startPos, targetPos, elapsedTime/animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gate.transform.position = targetPos; // set it to target after animation is done

        yield return new WaitForSeconds(1f); // chill a little

        secondaryCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true); // switch back

        gameObject.SetActive(false);

    }


}
