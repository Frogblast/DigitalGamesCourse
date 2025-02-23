using UnityEngine;
using UnityEngine.Events;

public class PuzzleTrigger : MonoBehaviour
{

    PuzzleWall.instance.OnPuzzleUpdated.AddListener();


}
