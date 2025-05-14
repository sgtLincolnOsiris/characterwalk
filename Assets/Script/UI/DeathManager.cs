using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathMenuUI;
    private PlayerRespawn playerRespawn;

    private void Start()
    {
        if (deathMenuUI != null)
        {
            deathMenuUI.SetActive(false);
        }

        // Cache player respawn component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerRespawn = player.GetComponent<PlayerRespawn>();
    }

    public void ShowDeathMenu()
    {
        if (deathMenuUI != null)
        {
            deathMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadHome(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void RespawnAtCheckpoint()
    {
        if (playerRespawn != null)
        {
            deathMenuUI.SetActive(false);
            Time.timeScale = 1f;
            playerRespawn.RespawnNow();
        }
        else
        {
            Debug.LogWarning("PlayerRespawn reference missing.");
        }
    }
}
