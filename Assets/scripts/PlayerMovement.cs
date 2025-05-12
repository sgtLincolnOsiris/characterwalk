using UnityEngine;
using System.Collections;
<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs

=======
using UnityEngine;
>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    float horizontalInput;
    [SerializeField] float moveSpeed = 5f;
    bool isFacingRight = true;
    [SerializeField] float jumpPower = 5f;
    bool isGrounded = false;
    int jumpCount = 0;
    [SerializeField] int maxJumps = 2;

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

<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
=======
    [Header("Audio")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip dashSound;
<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
=======
    [SerializeField] AudioClip walkingSound;
    [SerializeField] float stepInterval = 0.4f;
>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs

>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs
    Rigidbody2D rb;
    Animator animator;
<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
=======
    AudioSource audioSource;
    float stepTimer = 0f;
>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
        // Jumping
        if (!isDashing && Input.GetButtonDown("Jump") && jumpCount < maxJumps)
=======
        HandleWalkingSound();

        if (!isDashing && !isWallJumping && Input.GetButtonDown("Jump") && jumpCount < maxJumps)
>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpCount++;
            animator.SetBool("isJumping", true);
        }

        // Dash
        if (!isDashing && dashCooldownTimer <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        // Dash cooldown timer
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
        // Melee attack when not aiming
=======
        // Melee attack
>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs
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

<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
=======
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        jumpCount++;
        isGrounded = false;
        animator.SetBool("isJumping", true);

        if (jumpSound && audioSource)
            audioSource.PlayOneShot(jumpSound);
    }

    void WallJump()
    {
        isWallJumping = true;
        wallJumpTimer = wallJumpTime;
        wallJumpDirection = -transform.localScale.x;
        rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPowerX, wallJumpPowerY);
        animator.SetTrigger("jump");

        if (transform.localScale.x != wallJumpDirection)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }

        Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
    }

    void CancelWallJump()
    {
        isWallJumping = false;
    }

    void ProcessWallJumpCooldown()
    {
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0f)
            {
                isWallJumping = false;
            }
        }
    }

    bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        jumpCount = 0;
        animator.SetBool("isJumping", false);
    }

    IEnumerator Dash()
    {
        isDashing = true;

        if (dashSound && audioSource)
            audioSource.PlayOneShot(dashSound);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        if (dashSound && audioSource)
            audioSource.PlayOneShot(dashSound);

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
<<<<<<< Updated upstream:Assets/scripts/PlayerMovement.cs
            // Check if it's an enemy
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.TakeDamage(attackDamage);
            }
            // Check if it's a chicken
            else if (enemy.TryGetComponent<ChickenAI>(out ChickenAI chicken))
            {
                chicken.TakeDamage(attackDamage);
=======
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
>>>>>>> Stashed changes:Assets/Script/PlayerScripts/PlayerMovement.cs
            }
        }
    }

    void HandleWalkingSound()
    {
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        if (isMoving && isGrounded)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (walkingSound && audioSource)
                    audioSource.PlayOneShot(walkingSound);
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
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
