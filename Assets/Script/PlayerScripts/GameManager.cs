using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded; // Add this line
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Clean up listener
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to re-find pause menu in the new scene
        pauseMenuUI = GameObject.FindWithTag("PauseMenu"); // Ensure your pause menu is tagged with "PauseMenu"
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Listen for ESC key
        {
            if (isPaused)
            {
                ResumeGame(); // Resume if the game is paused
            }
            else
            {
                PauseGame(); // Pause if the game is running
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Pause the game
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
    }

    public void RespawnPlayer(PlayerRespawn player, float delay)
    {
        StartCoroutine(RespawnCoroutine(player, delay));
    }

    private IEnumerator RespawnCoroutine(PlayerRespawn player, float delay)
    {
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        player.transform.position = player.respawnPosition;
        player.gameObject.SetActive(true);
    }
}
