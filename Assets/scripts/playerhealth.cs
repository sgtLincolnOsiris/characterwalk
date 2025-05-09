using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;

    [Header("Optional Feedback")]
    [SerializeField] AudioClip healSound; // Optional healing sound
    [SerializeField] GameObject healEffectPrefab; // Optional visual effect

    Animator animator;
    PlayerMovement movement;
    AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        int prevHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        int actualLost = prevHealth - currentHealth;

        Debug.Log($"[Health Update] Player took {actualLost} damage. HP: {currentHealth}/{maxHealth}");

        animator?.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[Health Update] Player died!");
        animator?.SetTrigger("die");

        movement.enabled = false;
        this.enabled = false;
    }

    public void Heal(int amount)
    {
        int prevHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        int healedAmount = currentHealth - prevHealth;

        // Log the healing event
        Debug.Log($"[Health Update] Player healed by {healedAmount}. HP: {currentHealth}/{maxHealth}");

        // Trigger heal animation
        animator?.SetTrigger("heal");

        // Optional heal effect
        if (healEffectPrefab)
        {
            Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
        }

        // Play heal sound regardless of whether healing happened
        if (healSound && audioSource)
        {
            audioSource.PlayOneShot(healSound);
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
