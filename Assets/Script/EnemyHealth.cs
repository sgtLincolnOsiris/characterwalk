using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead { get; private set; } = false; // Public read, private write

    public AudioSource deathSound; // Sound for death
    public Animator animator; // Animator for death animation

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Prevent taking damage when already dead

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple death calls
        isDead = true;

        // Play death animation if animator exists
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Play death sound
        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Disable enemy behavior but wait for animation to complete
        GetComponent<Collider2D>().enabled = false; // Prevent further collisions
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Stop movement

        // Destroy after sound or animation finishes
        float destroyDelay = deathSound ? deathSound.clip.length : 0;
        if (animator != null)
        {
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                float animationDuration = clips[0].clip.length;
                destroyDelay = Mathf.Max(destroyDelay, animationDuration);
            }
        }

        Destroy(gameObject, destroyDelay);
    }
}
