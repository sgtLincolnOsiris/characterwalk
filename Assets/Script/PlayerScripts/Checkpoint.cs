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

        // Ensure there's a trigger collider
        if (GetComponent<Collider2D>() == null || !GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("Checkpoint needs a Collider2D set as trigger!", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug log to verify the trigger is working
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            ActivateCheckpoint(other.GetComponent<PlayerRespawn>());
        }
    }

    public void ActivateCheckpoint(PlayerRespawn player)
    {
        if (player == null)
        {
            Debug.LogWarning("PlayerRespawn component not found on player!", this);
            return;
        }

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
        Debug.Log("Checkpoint activated at: " + transform.position);
    }

    private void SetActiveState(bool state)
    {
        isActive = state;
        if (spriteRenderer != null)
            spriteRenderer.color = isActive ? activeColor : inactiveColor;
    }
}