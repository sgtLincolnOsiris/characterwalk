using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime;


    // Sound Effects
    public AudioSource alertSound;
    public AudioSource shootSound;

    private bool facingRight = true;
    private bool hasPlayedAlert = false;

    EnemyHealth health;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (player == null || health.isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            AlertAndAttack();
        }
        else
        {
            hasPlayedAlert = false;
        }
    }

    void AlertAndAttack()
    {
        if (health.isDead) return;

        FlipTowardsPlayer();

        if (!hasPlayedAlert && alertSound != null)
        {
            alertSound.Play();
            hasPlayedAlert = true;
        }

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void FlipTowardsPlayer()
    {
        if (health.isDead) return;

        bool playerIsRight = player.position.x > transform.position.x;

        if (playerIsRight && !facingRight || !playerIsRight && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Shoot()
    {
        if (health.isDead) return;

        if (shootSound != null)
        {
            shootSound.Play();
        }

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            // Calculate direction towards the player (for diagonal shooting)
            Vector2 direction = (player.position - firePoint.position).normalized;
            projectileScript.SetDirection(direction);
        }
    }
}
