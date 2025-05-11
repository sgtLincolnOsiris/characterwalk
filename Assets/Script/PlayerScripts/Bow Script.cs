using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject bowObject;            // The bow GameObject
    public GameObject arrowPrefab;          // The arrow prefab
    public Transform firePoint;             // Where the arrow spawns from
    public float arrowForce = 10f;

    [Header("Cooldown Settings")]
    public float cooldownTime = 0.5f;       // Time between shots
    private float lastShotTime = -Mathf.Infinity;

    [Header("Audio Settings")]
    public AudioClip shootSound;            // Sound to play when shooting the bow
    private AudioSource audioSource;        // Audio source component to play the sound

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        bowObject.SetActive(false); // Start with bow hidden
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the bow
    }

    private void Update()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - bowObject.transform.position).normalized;

        // Aim bow toward the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bowObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Show or hide bow when holding RMB
        if (Input.GetMouseButton(1)) // RMB held
        {
            bowObject.SetActive(true);

            // Fire arrow on LMB click if cooldown has passed
            if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + cooldownTime)
            {
                ShootArrow(direction);
                lastShotTime = Time.time; // Reset cooldown
            }
        }
        else
        {
            bowObject.SetActive(false);
        }
    }

    void ShootArrow(Vector2 direction)
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * arrowForce;
        }

        // Play the shoot sound when the arrow is fired
        if (shootSound && audioSource)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
