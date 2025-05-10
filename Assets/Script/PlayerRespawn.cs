using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector2 respawnPosition;
    public float respawnDelay = 2f;

    private GameManager gameManager;

    private void Start()
    {
        respawnPosition = transform.position;

        // Find GameManager in scene
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
            Debug.LogError("GameManager not found in scene!");
    }

    public void Die()
    {
        if (gameManager != null)
        {
            gameManager.RespawnPlayer(this, respawnDelay);
        }
        else
        {
            Debug.LogError("GameManager reference missing. Cannot respawn.");
        }
    }
}
