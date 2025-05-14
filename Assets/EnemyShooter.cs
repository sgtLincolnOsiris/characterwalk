using UnityEngine;
using System.Collections.Generic;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private int damageAmount = 15; // Changed to int
    [SerializeField] private float timeBetweenShots = 1.5f;
    [SerializeField] private Transform[] shootPoints;

    private float shootTimer;

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = timeBetweenShots;
        }
    }

    private void Shoot()
    {
        if (shootPoints.Length < 2) return;

        int index1 = Random.Range(0, shootPoints.Length);
        int index2 = (index1 + Random.Range(1, shootPoints.Length)) % shootPoints.Length;

        FireProjectile(shootPoints[index1]);
        FireProjectile(shootPoints[index2]);
    }

    private void FireProjectile(Transform shootPoint)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        var projectileScript = projectile.GetComponent<CannonballProjectile.Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(Vector2.down, projectileSpeed, damageAmount);
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Projectile component!");
        }
    }
}