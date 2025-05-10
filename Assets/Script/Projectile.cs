using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 20;
    public float lifetime = 5f;

    private Vector2 direction; // Travel direction

    void Start()
    {
        // Automatically destroy after a set time to prevent clutter
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile in the specified direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            PlayerHealth playerHealth = hitInfo.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (hitInfo.CompareTag("Ground") || hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    // Method to set the travel direction
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Ensure the direction is a unit vector

        // Adjust the rotation to face the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
