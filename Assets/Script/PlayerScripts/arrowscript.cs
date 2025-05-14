using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowLifetime = 30f;
    public int damage = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure continuous collision detection to prevent tunneling
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        RotateToVelocity();
        Destroy(gameObject, arrowLifetime);
    }

    void Update()
    {
        RotateToVelocity();
    }

    void RotateToVelocity()
    {
        if (rb != null && rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage enemy
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damage);
        }
        // Damage chicken
        else if (collision.TryGetComponent<ChickenAI>(out ChickenAI chicken))
        {
            chicken.TakeDamage(damage);
        }

        // Destroy arrow on contact with any non-trigger object (like ground or wall)
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}