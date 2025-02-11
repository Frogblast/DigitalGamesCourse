using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void OnRestartButton()
    {
        GameManager.Instance.RestartLevel();
    }
    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
