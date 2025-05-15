using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private int damageAmount = 15; // Changed to int
    [SerializeField] private float timeBetweenShots = 1.5f;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] public AudioClip shootSound;
    [SerializeField] private AudioSource audioSource;

    private float shootTimer;
    

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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

        audioSource.PlayOneShot(shootSound);
        FireProjectile(shootPoints[index1]);
        audioSource.PlayOneShot(shootSound, 0.5f);
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