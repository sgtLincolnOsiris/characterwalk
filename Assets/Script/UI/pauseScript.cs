using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI; // Assign this manually in Inspector

    private bool isPaused = false;

    private void Start()
    {
        // Optional fallback: try to find if not manually assigned
        if (pauseMenuUI == null)
        {
            pauseMenuUI = GameObject.FindWithTag("PauseMenu");

            if (pauseMenuUI == null)
            {
                Debug.LogWarning("Pause Menu UI not assigned and not found by tag.");
            }
        }

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide on start
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("PauseMenuUI not assigned.");
            return;
        }

        pauseMenuUI.SetActive(true); // Show menu first
        Time.timeScale = 0f;         // Then pause
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI == null) return;

        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f;          // Resume time
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadHome(string homeSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(homeSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
