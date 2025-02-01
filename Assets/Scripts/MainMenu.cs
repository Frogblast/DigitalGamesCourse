using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
