using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float deathDelay = 2f; // Time before destruction

    [Header("Death Actions")]
    [SerializeField] private GameObject[] activateOnDeath; // Objects to enable
    [SerializeField] private GameObject[] deactivateOnDeath; // Objects to disable

    [Header("Visuals")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject deathEffect;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    private int currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthBarUI.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        // Update UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            healthBarUI.SetActive(true);
        }

        // Play damage sound
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(DieWithDelay());
        }
    }

    private IEnumerator DieWithDelay()
    {
        isDead = true;

        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Trigger death effects
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Disable components (optional)
        GetComponent<Collider2D>().enabled = false;
        if (TryGetComponent(out ChargingEnemy chargingEnemy))
        {
            chargingEnemy.enabled = false;
        }

        // --- Activate/Deactivate GameObjects ---
        foreach (GameObject obj in activateOnDeath)
        {
            if (obj != null) obj.SetActive(true);
        }
        foreach (GameObject obj in deactivateOnDeath)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Wait before destroying
        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }

    public bool IsDead() => isDead;
}