using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    float horizontalInput;
    [SerializeField] float moveSpeed = 5f;
    bool isFacingRight = false;
    [SerializeField] float jumpPower = 5f;
    bool isGrounded = false;
    int jumpCount = 0;
    [SerializeField] int maxJumps = 5;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 50f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing = false;
    float dashCooldownTimer;

    [Header("Melee Attack Settings")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 1;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if (!isDashing && Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpCount++;
            animator.SetBool("isJumping", true);
        }

        if (!isDashing && dashCooldownTimer <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Melee attack when not aiming
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if (horizontalInput > 0f && !isFacingRight || horizontalInput < 0f && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        jumpCount = 0;
        animator.SetBool("isJumping", false);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

    void Attack()
    {
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Check if it's an enemy
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.TakeDamage(attackDamage);
            }
            // Check if it's a chicken
            else if (enemy.TryGetComponent<ChickenAI>(out ChickenAI chicken))
            {
                chicken.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
