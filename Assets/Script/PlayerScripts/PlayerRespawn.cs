using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector2 respawnPosition;
    public float respawnDelay = 2f;

    private GameManager gameManager;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        respawnPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

    public void RespawnNow()
    {
        // Move player to respawn point
        transform.position = respawnPosition;

        // Reset health
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        // Re-enable movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // Re-enable visuals if disabled
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        this.enabled = true;
    }
}
