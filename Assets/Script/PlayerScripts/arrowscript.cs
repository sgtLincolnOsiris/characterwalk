using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowLifetime = 30f;
    public int damage = 1;

    private Rigidbody2D rb;
    private Collider2D arrowCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<Collider2D>();

        // Rotate to match velocity
        RotateToVelocity();

        // Destroy arrow after 30 seconds if nothing is hit
        Destroy(gameObject, arrowLifetime);
    }

    void RotateToVelocity()
    {
        if (rb != null && rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void Update()
    {
        // Continuously rotate to match direction
        RotateToVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the arrow hits an enemy
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damage);
        }

        // Destroy arrow when it collides with anything (wall, enemy, etc.)
        Destroy(gameObject);
    }
}
