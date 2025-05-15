using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UI;


public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float deathDelay = 2f; // Time before destruction

    [Header("Death Actions")]
    [SerializeField] private GameObject[] activateOnDeath; // Objects to enable
    [SerializeField] private GameObject[] deactivateOnDeath; // Objects to disable

    [Header("Flash Settings")]
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;

    [Header("Visuals")]
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] public Image healthBar;


    [Header("Sound Effects")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] AudioClip hitSound;
    private AudioSource audioSource;
    Animator animator;
    Coroutine flashCoroutine;
    SpriteRenderer spriteRenderer;

    private int currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int prevHealth = currentHealth;
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        int actualLost = prevHealth - currentHealth;

        Debug.Log($"[Health Update] Player took {actualLost} damage. HP: {currentHealth}/{maxHealth}");

        if (hitSound && audioSource)
            audioSource.PlayOneShot(hitSound);

        UpdateHealthBar();
        animator?.SetTrigger("hurt");

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRed());

        // Play damage sound
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(DieWithDelay());
        }
    }

    IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private IEnumerator DieWithDelay()
    {
        isDead = true;

        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Trigger death effects
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Disable components (optional)
        GetComponent<Collider2D>().enabled = false;
        if (TryGetComponent(out ChargingEnemy chargingEnemy))
        {
            chargingEnemy.enabled = false;
        }

        // --- Activate/Deactivate GameObjects ---
        foreach (GameObject obj in activateOnDeath)
        {
            if (obj != null) obj.SetActive(true);
        }
        foreach (GameObject obj in deactivateOnDeath)
        {
            if (obj != null) obj.SetActive(false);
        }

        animator.SetTrigger("Death");
        // Wait before destroying
        yield return new WaitForSeconds(deathDelay);
      
    }

    public bool IsDead() => isDead;
}