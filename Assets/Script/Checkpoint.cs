using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActive = false;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.gray;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetActiveState(isActive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint(other.GetComponent<PlayerRespawn>());
        }
    }

    public void ActivateCheckpoint(PlayerRespawn player)
    {
        if (player == null) return;

        // Set respawn position
        player.respawnPosition = transform.position;

        // Deactivate other checkpoints
        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint cp in allCheckpoints)
        {
            cp.SetActiveState(false);
        }

        // Activate this checkpoint
        SetActiveState(true);
    }

    private void SetActiveState(bool state)
    {
        isActive = state;
        if (spriteRenderer != null)
            spriteRenderer.color = isActive ? activeColor : inactiveColor;
    }
}
