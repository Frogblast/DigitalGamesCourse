using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject winningMenu;
    [SerializeField]
    private GameObject settingsMenu;

    private void Start()
    {
        gameOverMenu.SetActive(false);
        winningMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerDeath += OpenGameOverMenu;
        EventManager.OnWinningGame += OpenWinningMenu;
        EventManager.OnOpenMenu += OpenSettingsMenu;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= OpenGameOverMenu;
        EventManager.OnWinningGame -= OpenWinningMenu;
        EventManager.OnOpenMenu -= OpenSettingsMenu;
    }

    private void OpenWinningMenu()
    {
        winningMenu.SetActive(true);
        EventManager.SetAnyMenuOpen(true);
    }

    private void OpenGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        EventManager.SetAnyMenuOpen(true);
    }

    private void OpenSettingsMenu()
    {
        Debug.Log("invoke settings");

        if (EventManager.IsAnyMenuOpen() && !settingsMenu.activeSelf) // if other ui is open and its not the setting ui
        {
            Debug.Log("Other menu is open");
            return;
        } // avoid covering the other guis if they are active
        
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        EventManager.SetAnyMenuOpen(settingsMenu.activeSelf); // Update the state that a menu is now open

    }
}
