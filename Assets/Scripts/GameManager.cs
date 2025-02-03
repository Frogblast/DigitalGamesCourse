using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GameManager persists between scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance of GameManager exists
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
    }

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
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
    }

    internal void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
