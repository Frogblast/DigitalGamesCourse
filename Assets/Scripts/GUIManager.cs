using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverMenu;

    private void Start()
    {
        gameOverMenu.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerDeath += OpenGameOverMenu;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= OpenGameOverMenu;
    }
    private void OpenGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }
}
