using System.Collections;
using System.Linq;
using UnityEngine;

public class CannonballSpawner : MonoBehaviour
{
    public GameObject cannonballPrefab; // Cannonball prefab
    public Transform[] spawnPoints; // 5 predefined spawn points
    public float interval = 2f; // Time between each cannonball fall
    public float launchForce = 10f; // Force of the cannonball launch
    public int cannonballDamage = 25; // Damage dealt by the cannonball

    // Sound Effects
    public AudioSource shootSound;

    // Timing settings
    public float shotDelay = 0.5f; // Time between each shot
    public float reloadTime = 5f; // Time before reloading

    private bool isReloading = false;

    private void Update()
    {
        if (!isReloading)
        {
            StartCoroutine(SpawnCannonballsWithDelay());
        }
    }

    private IEnumerator SpawnCannonballsWithDelay()
    {
        isReloading = true;

        // Randomly select 3 unique spawn points
        Transform[] selectedPoints = spawnPoints.OrderBy(x => Random.value).Take(3).ToArray();

        foreach (Transform spawnPoint in selectedPoints)
        {
            // Instantiate the cannonball at the chosen spawn point
            GameObject cannonball = Instantiate(cannonballPrefab, spawnPoint.position, Quaternion.identity);

            // Apply projectile force
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.down * launchForce, ForceMode.Impulse);
            }

            if (shootSound != null)
            {
                shootSound.Play();
            }

            // Attach damage script to cannonball
            CannonballDamage damageComponent = cannonball.AddComponent<CannonballDamage>();
            damageComponent.damage = cannonballDamage;

            // Wait for shotDelay seconds before the next shot
            yield return new WaitForSeconds(shotDelay);
        }

        // Wait for reloadTime before reloading
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}

// Separate script for handling cannonball damage
public class CannonballDamage : MonoBehaviour
{
    public int damage = 25;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Destroy the cannonball on impact
            Destroy(gameObject);
        }
    }
}
