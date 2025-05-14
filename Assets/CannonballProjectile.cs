using UnityEngine;

namespace CannonballProjectile
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private Vector2 direction;
        private float speed;
        private int damage; // Changed to int to match caller
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }

        public void Initialize(Vector2 dir, float spd, int dmg) // Changed dmg to int
        {
            direction = dir.normalized;
            speed = spd;
            damage = dmg;

            if (direction.y < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
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
    }
}