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

    [SerializeField] private GameObject goldStaute;
    [SerializeField] private GameObject keyStatue;

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

    void rotateKeyStatue() // Rotates the statue to indicate a correct choice
    {
        keyStatue.transform.eulerAngles = new Vector3(0, 0, -90);
    }

    void RotateGoldStatue()
    {
        goldStaute.transform.eulerAngles = new Vector3(0, 0, -90);
    }


    void OnKeyTriggerEntered(Collider collider)
    {
        if (collider.tag == "Key")
        {
            isKeyPlaced = true;

            rotateKeyStatue(); 
            puzzleSolved();
        }
    }

    void OnGoldbarTriggerEntered(Collider collider)
    {
        if (collider.tag == "GoldBar")
        { 
            isGoldbarPlaced = true;

            RotateGoldStatue();
            puzzleSolved(); // Begin animation if puzzle is solved
        }
    }

    /*private void Update()
    {
        if (isKeyPlaced == true && isGoldbarPlaced == true)
        {
            StartCoroutine(HandleUnlock());
            
        }
    }*/

    void puzzleSolved() // If puzzle is solved, this starts the animation
    {
        if (isGoldbarPlaced == true && isKeyPlaced == true)
        {
            StartCoroutine(HandleUnlock());
        }
    }

    private void disableBarrier() // Removes the barrier around chest
    {
        gameObject.SetActive(false);
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

        disableBarrier();

    }


}
