using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;
    private Rigidbody2D rb;
    bool isDead = false;

    // Property version (use as playerHealth.IsDead)

    // Method version (use as playerHealth.IsDead())
    public bool IsDead() => isDead;

    [Header("Optional Feedback")]
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hitSound;

    [Header("UI")]
    public Image healthBar;
    public DeathMenuManager deathMenuManager;

    Animator animator;
    PlayerMovement movement;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;

    [Header("Flash Settings")]
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;

    [Header("Pulse Settings")]
    [SerializeField] Color pulseColor = Color.green;
    [SerializeField] float pulseDuration = 0.5f;
    [SerializeField] float pulseSpeed = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int prevHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        int actualLost = prevHealth - currentHealth;

        Debug.Log($"[Health Update] Player took {actualLost} damage. HP: {currentHealth}/{maxHealth}");

        if (hitSound && audioSource)
        {
            audioSource.PlayOneShot(hitSound);
        }

        UpdateHealthBar();
        animator?.SetTrigger("hurt");
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);
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

    public void Heal(int amount)
    {
        if (isDead) return;

        int prevHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        int healedAmount = currentHealth - prevHealth;

        Debug.Log($"[Health Update] Player healed by {healedAmount}. HP: {currentHealth}/{maxHealth}");

        UpdateHealthBar();
        animator?.SetTrigger("heal");

        StartCoroutine(PulseGreen());

        if (healSound && audioSource)
        {
            audioSource.PlayOneShot(healSound);
        }
    }

    IEnumerator PulseGreen()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            float elapsedTime = 0f;
            while (elapsedTime < pulseDuration)
            {
                spriteRenderer.color = Color.Lerp(originalColor, pulseColor, Mathf.PingPong(elapsedTime * pulseSpeed, 1));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
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

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[Health Update] Player died!");
        animator?.SetTrigger("die");

        if (deathSound && audioSource)
        {
            audioSource.PlayOneShot(deathSound);
        }

        movement.enabled = false;

        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        StartCoroutine(ShowDeathMenuAfterDelay(2f));
    }

    private IEnumerator ShowDeathMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        deathMenuManager?.ShowDeathMenu();
        this.enabled = false;
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}