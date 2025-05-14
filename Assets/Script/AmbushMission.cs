using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class TriggerWithSound : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("Only objects with this tag can trigger the activation")]
    public string triggerTag = "Player";
    public float activationDelay = 0f;
    public float deactivationDelay = 0f; // New delay for deactivation
    public bool disableAfterTrigger = true;

    [Header("Object Control")]
    [Tooltip("Objects to activate when triggered")]
    public List<GameObject> objectsToActivate;
    [Tooltip("Objects to deactivate when triggered")]
    public List<GameObject> objectsToDeactivate;

    [Header("Sound Effects")]
    public AudioClip triggerSound;
    [Range(0f, 1f)] public float volume = 1f;
    public bool playSoundOnActivation = true;
    public bool playSoundOnDeactivation = false;

    private AudioSource audioSource;
    private bool isActivated = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag(triggerTag))
        {
            StartCoroutine(TriggerSequence());
        }
    }

    private System.Collections.IEnumerator TriggerSequence()
    {
        isActivated = true;

        // Play activation sound if configured
        if (playSoundOnActivation && triggerSound != null)
        {
            audioSource.PlayOneShot(triggerSound, volume);
        }

        // Wait for activation delay if specified
        if (activationDelay > 0f)
        {
            yield return new WaitForSeconds(activationDelay);
        }

        // Activate specified objects
        foreach (var obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Wait for deactivation delay if specified
        if (deactivationDelay > 0f)
        {
            yield return new WaitForSeconds(deactivationDelay);
        }

        // Deactivate specified objects
        foreach (var obj in objectsToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Play deactivation sound if configured
        if (playSoundOnDeactivation && triggerSound != null)
        {
            audioSource.PlayOneShot(triggerSound, volume);
        }


    }
}