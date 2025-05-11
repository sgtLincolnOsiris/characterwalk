using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public GameObject targetToEnable;      // The object to disable
    public GameObject triggeringObject;     // The specific GameObject to detect
    public AudioClip triggerSound;          // Sound to play

    private AudioSource audioSource;
    private bool triggered = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && triggerSound != null)
        {
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

        // Disable the assigned GameObject
        if (targetToEnable != null)
            targetToEnable.SetActive(true);

        // Play sound
        if (triggerSound != null && audioSource != null)
            audioSource.PlayOneShot(triggerSound);
    }
}
