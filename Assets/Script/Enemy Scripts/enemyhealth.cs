using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    public bool isDead { get; private set; } = false;

    Animator animator;
    Rigidbody2D rb;
    Collider2D enemyCollider;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>(); // Cache the collider
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

        // Optional: Disable the AI to stop further logic
        // GetComponent<EnemyAI>().enabled = false;
    }
}
