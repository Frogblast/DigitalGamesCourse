using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManagerPrefab;

    private void Awake()
    {
        if(GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }
}
