using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Ranged Enemy Settings")]
    [SerializeField] float detectionRange = 5f;  // Range to detect player
    [SerializeField] float attackCooldown = 1f;  // Time between shots
    [SerializeField] GameObject projectilePrefab;  // Projectile prefab
    [SerializeField] Transform firePoint;  // Point where projectiles are fired
    [SerializeField] int maxHealth = 3;  // Enemy health

    private float attackTimer = 0f;
    private int currentHealth;
    private Animator animator;
    private Transform player;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;  // Find player by tag
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            AttackPlayer();
        }
        else
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isIdle", true);  // Play idle animation
        }
    }

    void AttackPlayer()
    {
        animator.SetBool("isIdle", false);  // Stop idle animation
        animator.SetBool("isShooting", true);  // Play shooting animation

        if (attackTimer <= 0f)
        {
            ShootProjectile();
            attackTimer = attackCooldown;  // Reset attack cooldown
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Instantiate projectile at firePoint
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("hurt");  // Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isShooting", false);  // Stop shooting animation
        animator.SetTrigger("die");  // Play die animation

        // Disable enemy's collider and any further actions
        GetComponent<Collider2D>().enabled = false;

        // Destroy enemy after some time to allow death animation to play
        Destroy(gameObject, 2f);  // Adjust time as needed
    }
}
