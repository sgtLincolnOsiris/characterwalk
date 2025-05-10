using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAbilitySwitch : MonoBehaviour
{
    private MonoBehaviour firstAbility; // The first ability (e.g., charge ability)
    private MonoBehaviour secondAbility; // The second ability (e.g., pounce ability)

    public int maxHealth = 100; // Boss max health
    private int currentHealth;

    public Slider healthBar; // Reference to the health bar UI

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        firstAbility = GetComponent<BossChargeBackAndForth>(); // Assuming your first ability script
        secondAbility = GetComponent<BossPounce>(); // Assuming your second ability script

        EnableFirstAbility();
    }

    private void Update()
    {
        if (currentHealth <= maxHealth / 2)
        {
            EnableSecondAbility();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void EnableFirstAbility()
    {
        if (firstAbility != null) firstAbility.enabled = true;
        if (secondAbility != null) secondAbility.enabled = false;
    }

    private void EnableSecondAbility()
    {
        if (firstAbility != null) firstAbility.enabled = false;
        if (secondAbility != null) secondAbility.enabled = true;
    }

    private void Die()
    {
        Debug.Log("Boss has died.");
        Destroy(gameObject);
    }
}