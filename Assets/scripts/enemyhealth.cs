using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    public bool isDead { get; private set; } = false;

    Animator animator;
    Rigidbody2D rb;
    Collider2D enemyCollider;
<<<<<<< Updated upstream:Assets/scripts/enemyhealth.cs
=======
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    [Header("Flash Settings")]
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;
>>>>>>> Stashed changes:Assets/Script/Enemy Scripts/enemyhealth.cs

    [Header("Audio")]
    [SerializeField] AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
<<<<<<< Updated upstream:Assets/scripts/enemyhealth.cs
        enemyCollider = GetComponent<Collider2D>(); // Cache the collider
=======
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Required to play sound
>>>>>>> Stashed changes:Assets/Script/Enemy Scripts/enemyhealth.cs
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}");

        animator?.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} died!");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Disable physics interaction

        // Stop movement and animations
        animator?.SetBool("isWalking", false);
        animator?.SetBool("isAttacking", false);
        animator?.SetTrigger("Die");

        // Disable collider so player cannot interact with dead enemy
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

<<<<<<< Updated upstream:Assets/scripts/enemyhealth.cs
        // Optional: Disable the AI to stop further logic
        // GetComponent<EnemyAI>().enabled = false;
=======
        if (deathSound && audioSource)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Optional: Disable AI
        // GetComponent<EnemyAI>()?.enabled = false;
>>>>>>> Stashed changes:Assets/Script/Enemy Scripts/enemyhealth.cs
    }
}
