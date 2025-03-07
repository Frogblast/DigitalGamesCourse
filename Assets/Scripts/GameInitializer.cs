using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManagerPrefab;
    [SerializeField]
    private AudioManager audioManagerPrefab;

    private void Awake()
    {
        if(GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
        if (AudioManager.Instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
}
