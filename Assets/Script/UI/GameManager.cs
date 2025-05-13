using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // You can reinitialize scene-specific logic here if needed
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
