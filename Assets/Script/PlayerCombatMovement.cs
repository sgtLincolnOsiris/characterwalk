using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Combat")]
    public GameObject meleeAttackArea;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        if (Input.GetMouseButtonDown(0)) // Left click for melee
            MeleeAttack();

        if (Input.GetMouseButtonDown(1)) // Right click to shoot
            ShootProjectile();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    void MeleeAttack()
    {
        meleeAttackArea.SetActive(true);
        Invoke(nameof(DisableMelee), 0.2f); // Attack duration
    }

    void DisableMelee()
    {
        meleeAttackArea.SetActive(false);
    }

    void ShootProjectile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        projRb.linearVelocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }
}
