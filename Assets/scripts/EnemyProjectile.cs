using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] float speed = 10f;  // Speed of the projectile
    [SerializeField] int damage = 1;  // Damage dealt by the projectile
    [SerializeField] float lifetime = 5f;  // Time after which the projectile is destroyed if not hitting anything
    [SerializeField] bool isFacingRight = true;  // Direction the projectile is moving towards

    private Rigidbody2D rb;
    private float lifetimeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifetimeTimer = lifetime;
        Destroy(gameObject, lifetime);  // Destroy projectile after its lifetime ends
    }

    void Update()
    {
        // Move the projectile
        if (isFacingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);  // Move right
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);  // Move left
        }

        // Decrease lifetime timer
        lifetimeTimer -= Time.deltaTime;

        if (lifetimeTimer <= 0f)
        {
            Destroy(gameObject);  // Destroy projectile if lifetime ends
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with player or enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // Deal damage to player
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);  // Deal damage to enemy
            }
        }

        // Destroy the projectile after it collides
        Destroy(gameObject);
    }

    // Method to set the direction of the projectile
    public void SetDirection(bool facingRight)
    {
        isFacingRight = facingRight;
    }
}
