using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class TimerTriggerController : MonoBehaviour
{
    [Header("Timer Settings")]
    public float countdownDuration = 5f;
    public TMP_Text countdownText;
    public bool resetOnPlayerDeath = true;

    [Header("Scene Control")]
    public string sceneToLoad;
    public string sceneToUnload;

    [Header("Visual Feedback")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float warningThreshold = 3f;
    public AudioClip countdownSound;
    public AudioClip timeoutSound;

    private float currentTime;
    private bool playerInTrigger;
    private bool sceneLoadingInProgress;
    private AudioSource audioSource;
    private GameObject player;
    private PlayerHealth playerHealth; // Assume you have a PlayerHealth component

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInTrigger && !sceneLoadingInProgress)
        {
            // Check for player death if reset is enabled
            if (resetOnPlayerDeath && playerHealth != null && playerHealth.IsDead())
            {
                ResetCountdown();
                return;
            }

            currentTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (currentTime <= 0)
            {
                OnCountdownFinished();
            }
        }
    }

    private void UpdateCountdownDisplay()
    {
        int displayTime = Mathf.CeilToInt(currentTime);
        countdownText.text = displayTime.ToString();

        // Visual feedback when time is running out
        countdownText.color = currentTime <= warningThreshold ? warningColor : normalColor;

        // Play sound at whole seconds
        if (Mathf.FloorToInt(currentTime) < Mathf.FloorToInt(currentTime + Time.deltaTime))
        {
            PlayCountdownSound();
        }
    }

    private void PlayCountdownSound()
    {
        if (audioSource != null && countdownSound != null)
        {
            audioSource.PlayOneShot(countdownSound);
        }
    }

    private void OnCountdownFinished()
    {
        countdownText.text = "0";
        playerInTrigger = false;
        sceneLoadingInProgress = true;

        if (timeoutSound != null)
        {
            audioSource.PlayOneShot(timeoutSound);
        }

        StartCoroutine(HandleSceneTransition());
        countdownText.gameObject.SetActive(false);
    }

    private IEnumerator HandleSceneTransition()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // Check if scene is already loaded
            if (!SceneManager.GetSceneByName(sceneToLoad).isLoaded)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
                yield return loadOperation;
            }
        }

        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            // Check if scene is loaded before trying to unload
            if (SceneManager.GetSceneByName(sceneToUnload).isLoaded)
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
                yield return unloadOperation;
            }
        }

        sceneLoadingInProgress = false;
    }

    private void ResetCountdown()
    {
        currentTime = countdownDuration;
        countdownText.gameObject.SetActive(false);
        playerInTrigger = false;
        StopAllCoroutines();
        sceneLoadingInProgress = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            currentTime = countdownDuration;
            countdownText.gameObject.SetActive(true);
            UpdateCountdownDisplay();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetCountdown();
        }
    }

    // Optional: Call this from your player's death event
    public void OnPlayerDeath()
    {
        if (resetOnPlayerDeath)
        {
            ResetCountdown();
        }
    }
}