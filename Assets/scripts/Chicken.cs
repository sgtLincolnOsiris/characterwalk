using UnityEngine;

public class ChickenAI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 1;
    int currentHealth;
    bool isDead = false;

    [Header("Run Settings")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float detectRange = 5f;

    [Header("Heal On Death")]
    [SerializeField] int healAmount = 1;

    Animator animator;
    Transform player;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectRange)
        {
            RunAway();
        }
        else
        {
            StopRunning();
        }
    }

    void RunAway()
    {
        animator.SetBool("isRunning", true);

        Vector2 dir = (transform.position - player.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * runSpeed, rb.linearVelocity.y);

        // Flip sprite direction based on movement
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = dir.x < 0f;
        }
    }

    void StopRunning()
    {
        if (animator.GetBool("isRunning"))
        {
            animator.SetBool("isRunning", false);
        }

        if (rb.linearVelocity.x != 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        Debug.Log("Chicken took damage!");

        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("die");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        // Heal player
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
        }

        Invoke(nameof(DestroySelf), 3f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
