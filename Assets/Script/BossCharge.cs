using UnityEngine;
using System.Collections;

public class ChargingEnemy : MonoBehaviour
{
    [Header("Charge Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Timing Settings")]
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float roarDelay = 1f;
    [SerializeField] private float restDuration = 3f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip chargeSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float knockbackForce = 5f;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D hurtbox;

    private Vector2 targetPosition;
    private bool isCharging = false;
    private Coroutine chargeSequence;
    private bool isAlive = true;


    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Charge points not assigned!");
            enabled = false;
            return;
        }

        // Initialize audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Start the charge sequence
        chargeSequence = StartCoroutine(ChargeSequenceLoop());
    }

    private void Update()
    {
        if (isCharging)
        {
            // Move towards target
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                chargeSpeed * Time.deltaTime
            );

            // Check if reached target
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isCharging = false;
                animator.SetBool("Charge", false);
                hurtbox.enabled = false;
            }
        }
    }

    private IEnumerator ChargeSequenceLoop()
    {
        while (isAlive)
        {
            // --- STAGE 1: Charge to A ---
            targetPosition = pointA.position;
            FlipSpriteToFace(targetPosition);

            // Roar & delay
            animator.SetTrigger("Roar");
            PlaySound(roarSound);
            yield return new WaitForSeconds(roarDelay);

            // Charge to A
            PlaySound(chargeSound);
            StartCharge();
            yield return new WaitUntil(() => !isCharging);

            // --- REST PHASE ---
            FlipSpriteToFace(targetPosition);
            yield return new WaitForSeconds(restDuration);

            // --- STAGE 2: Charge to B ---
            targetPosition = pointB.position;
            FlipSpriteToFace(targetPosition);

            // Roar & delay
            animator.SetTrigger("Roar");
            PlaySound(roarSound);
            yield return new WaitForSeconds(roarDelay);

            // Charge to B
            PlaySound(chargeSound);
            StartCharge();
            yield return new WaitUntil(() => !isCharging);

            // --- REST PHASE ---
            FlipSpriteToFace(targetPosition);
            yield return new WaitForSeconds(restDuration);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        hurtbox.enabled = true;
        animator.SetTrigger("Charge");
    }

    private void FlipSpriteToFace(Vector2 target)
    {
        transform.localScale = new Vector3(
            target.x > transform.position.x ? -3 : 3,3,3
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCharging && collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                player.ApplyKnockback(knockbackDir * knockbackForce);
            }
        }
    }

    public void Die()
    {
        isAlive = false;
        if (chargeSequence != null)
            StopCoroutine(chargeSequence);


        // Play death animation, effects, etc.
        Destroy(gameObject, 2f); // Optional delay for death animation
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.position, 0.5f);
        }
    }
}