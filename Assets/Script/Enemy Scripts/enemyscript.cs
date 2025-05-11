using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float attackRange = 1f;

    [Header("Attack")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int attackDamage = 1;
    float attackTimer = 0f;

    Rigidbody2D rb;
    Animator animator;
    Transform player;
    EnemyHealth health;
    public PlayerHealth pHealth;
    public float damage;

    bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // Don't perform any actions if dead
        if (player == null || health.isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            ChasePlayer();
            animator?.SetBool("isAttacking", false);
        }
        else if (distance <= attackRange)
        {
            Attack();
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            animator?.SetBool("isWalking", false);
            animator?.SetBool("isAttacking", false);
        }

        attackTimer -= Time.deltaTime;
    }


    void ChasePlayer()
    {
        if (health.isDead) return;

        animator?.SetBool("isWalking", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            Flip();
        }
    }

    void Attack()
    {
        if (health.isDead) return;

        rb.linearVelocity = Vector2.zero;
        animator?.SetBool("isWalking", false);
        animator?.SetBool("isAttacking", true);

        if (attackTimer <= 0f)
        {
            if (player.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(attackDamage);
            }

            attackTimer = attackCooldown;
        }


    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
