using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject winningMenu;

    private void Start()
    {
        gameOverMenu.SetActive(false);
        winningMenu.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerDeath += OpenGameOverMenu;
        EventManager.OnWinningGame += OpenWinningMenu;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= OpenGameOverMenu;
        EventManager.OnWinningGame -= OpenWinningMenu;
    }

    private void OpenWinningMenu()
    {
        winningMenu.SetActive(true);
    }

    private void OpenGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }
}
