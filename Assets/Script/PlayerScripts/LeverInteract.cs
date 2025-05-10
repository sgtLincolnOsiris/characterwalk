using UnityEngine;

public class LeverInteract : MonoBehaviour
{
    public GameObject targetToDisable;      // Assign in Inspector
    public AudioClip triggerSound;          // Assign sound in Inspector
    private AudioSource audioSource;
    private bool triggered = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && triggerSound != null)
        {
            // Auto-add AudioSource if missing
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

        // Flip this GameObject
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Disable the assigned target
        if (targetToDisable != null)
            targetToDisable.SetActive(false);

        // Play sound
        if (triggerSound != null && audioSource != null)
            audioSource.PlayOneShot(triggerSound);
    }
}
