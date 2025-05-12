using UnityEngine;
using System.Collections;

public class DrowningZone : MonoBehaviour
{
    [SerializeField] float delayBeforeDamage = 5f;
    [SerializeField] float damageInterval = 1f;
    [SerializeField] int damagePerTick = 1;

    private Coroutine drowningCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            drowningCoroutine = StartCoroutine(DrowningRoutine(other.GetComponent<PlayerHealth>()));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && drowningCoroutine != null)
        {
            StopCoroutine(drowningCoroutine);
            drowningCoroutine = null;
        }
    }

    private IEnumerator DrowningRoutine(PlayerHealth playerHealth)
    {
        if (playerHealth == null) yield break;

        // Wait before starting to damage
        yield return new WaitForSeconds(delayBeforeDamage);

        while (true)
        {
            if (playerHealth.GetCurrentHealth() > 0)
            {
                playerHealth.TakeDamage(damagePerTick);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }
}
