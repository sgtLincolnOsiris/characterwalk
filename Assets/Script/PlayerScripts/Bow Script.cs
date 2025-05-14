using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject bowObject;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowForce = 10f;

    [Header("Cooldown Settings")]
    public float cooldownTime = 0.5f;
    private float lastShotTime = -Mathf.Infinity;

    [Header("Audio Settings")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Player Reference")]
    public Transform player; // Assign in Inspector
    private PlayerMovement playerMovement;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        bowObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (player != null)
        {
            bowObject.transform.position = player.position;
        }

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - bowObject.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bowObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(1))
        {
            bowObject.SetActive(true);

            if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + cooldownTime)
            {
                if (playerMovement != null && playerMovement.HasArrows())
                {
                    ShootArrow(direction);
                    playerMovement.UseArrow();
                    lastShotTime = Time.time;
                }
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

        if (shootSound && audioSource)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
