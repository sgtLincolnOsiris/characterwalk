using UnityEngine;
using System.Collections;



public class BowController : MonoBehaviour
{
    [Header("Bow Settings")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float arrowSpeed = 10f;
    [SerializeField] float arrowLifetime = 5f;

    [Header("Ammo Settings")]
    [SerializeField] int maxAmmo = 5;
    [SerializeField] int currentAmmo;

    [Header("Audio")]
    [SerializeField] AudioClip shootSound;

    AudioSource audioSource;

    void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        if (shootPoint != null && arrowPrefab != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * arrowSpeed, direction.y * arrowSpeed);

                // Destroy the arrow after a set amount of time
                Destroy(arrow, arrowLifetime);
            }

            if (audioSource && shootSound)
            {
                audioSource.PlayOneShot(shootSound);
            }

            currentAmmo--;
        }
    }

    public void Reload(int ammoAmount)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammoAmount, maxAmmo);
    }
}
