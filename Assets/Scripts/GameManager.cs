using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnPlayerDeath += GameOver;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= GameOver;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the game...
    }
}
