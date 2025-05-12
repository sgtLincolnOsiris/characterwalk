using System.Collections;
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

    [Header("Audio")]
    [SerializeField] AudioClip deathSound;

    [Header("Flash Settings")]
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;

    Animator animator;
    Transform player;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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

        if (spriteRenderer != null)
            spriteRenderer.flipX = dir.x < 0f;
    }

    void StopRunning()
    {
        if (animator.GetBool("isRunning"))
            animator.SetBool("isRunning", false);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("hurt");

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
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

    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("die");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        if (deathSound && audioSource)
            audioSource.PlayOneShot(deathSound);

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
