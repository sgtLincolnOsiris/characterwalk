using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    public bool isDead { get; private set; } = false;

    Animator animator;
    Rigidbody2D rb;
    Collider2D enemyCollider;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    [Header("Flash Settings")]
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;

    [Header("Audio")]
    [SerializeField] AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Required to play sound
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}");

        animator?.SetTrigger("hurt");
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} died!");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator?.SetBool("isWalking", false);
        animator?.SetBool("isAttacking", false);
        animator?.SetTrigger("Die");

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (deathSound && audioSource)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Optional: Disable AI
        // GetComponent<EnemyAI>()?.enabled = false;
    }
}
