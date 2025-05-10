using UnityEngine;

public class OutOfBoundsDeath : MonoBehaviour
{
    public string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            PlayerRespawn player = other.GetComponent<PlayerRespawn>();
            if (player != null)
            {
                player.Die();
            }
            else
            {
                Debug.LogWarning("No PlayerRespawn script found on target!");
            }
        }
    }
}
