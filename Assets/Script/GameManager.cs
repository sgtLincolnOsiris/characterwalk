using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player; // Reference to the player

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager persistent
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance
        }
    }

    // Respawn system
    public void RespawnPlayer(PlayerRespawn player, float delay)
    {
        StartCoroutine(RespawnCoroutine(player, delay));
    }

    private IEnumerator RespawnCoroutine(PlayerRespawn player, float delay)
    {
        player.gameObject.SetActive(false); // Hide
        yield return new WaitForSeconds(delay);
        player.transform.position = player.respawnPosition;
        player.gameObject.SetActive(true); // Respawn
    }
}
