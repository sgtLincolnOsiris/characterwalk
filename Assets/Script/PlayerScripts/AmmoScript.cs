using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    [Header("Arrow Pickup Settings")]
    public int arrowsToAdd = 5;

    [Header("Audio Settings")]
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.AddArrows(arrowsToAdd);
                PlayPickupSound();
                Destroy(gameObject); // Safe to destroy now
            }
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSound)
        {
            // This plays the clip at the object's position, using a temporary AudioSource
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }
}
