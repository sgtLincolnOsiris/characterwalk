using System.Collections;
using UnityEngine;

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

    [Header("Wall Jump Settings")]
    [SerializeField] Transform wallCheckPos;
    [SerializeField] Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float wallJumpPowerX = 5f;
    [SerializeField] float wallJumpPowerY = 10f;
    float wallJumpDirection;
    bool isWallJumping;
    [SerializeField] float wallJumpTime = 0.5f;
    float wallJumpTimer;

    [Header("Melee Attack Settings")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 1;

    [Header("Arrow Settings")]
    [SerializeField] int maxArrows = 10;
    int currentArrows = 0;

    [Header("Audio")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip walkingSound;
    [SerializeField] float stepInterval = 0.4f;

    Rigidbody2D rb;
    Animator animator;
    AudioSource audioSource;
    float stepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();
        ProcessWallJumpCooldown();

        HandleWalkingSound();

        if (!isDashing && !isWallJumping && Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            if (WallCheck())
            {
                WallJump();
            }
            else
            {
                Jump();
            }
        }

        if (!isDashing && dashCooldownTimer <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing && !isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

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

    void FlipSprite()
    {
        if ((horizontalInput > 0f && !isFacingRight) || (horizontalInput < 0f && isFacingRight))
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

        if (attackSound && audioSource)
            audioSource.PlayOneShot(attackSound);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.TakeDamage(attackDamage);
            }
            else if (enemy.TryGetComponent<ChickenAI>(out ChickenAI chicken))
            {
                chicken.TakeDamage(attackDamage);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    // ---- Arrow Methods ----

    public void AddArrows(int amount)
    {
        currentArrows = Mathf.Clamp(currentArrows + amount, 0, maxArrows);
    }

    public bool HasArrows()
    {
        return currentArrows > 0;
    }

    public void UseArrow()
    {
        if (currentArrows > 0)
            currentArrows--;
    }

    public int GetArrowCount()
    {
        return currentArrows;
    }
}
