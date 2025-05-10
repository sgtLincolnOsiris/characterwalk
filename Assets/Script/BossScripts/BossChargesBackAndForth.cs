using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeBackAndForth : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public float chargeSpeed = 10f; // Speed of the boss charge
    public float knockUpForce = 5f; // Force to knock up the player
    public float disableDuration = 0.5f; // Duration to disable player movement
    public int damage = 20; // Damage dealt by the charge

    public Transform pointA; // First point
    public Transform pointB; // Second point
    public int chargeCount = 3; // Number of charges in one set
    public float restDelay = 2f; // Delay before next set of charges

    private Rigidbody2D playerRb;
    private PlayerController playerController;

    private void Start()
    {
        player = GameManager.Instance.player; // Get player from GameManager
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();

        StartCoroutine(ChargeBackAndForth());
    }

    private IEnumerator ChargeBackAndForth()
    {
        while (true)
        {
            for (int i = 0; i < chargeCount; i++)
            {
                yield return StartCoroutine(PerformCharge(pointA.position, pointB.position));
                yield return StartCoroutine(PerformCharge(pointB.position, pointA.position));
            }

            yield return new WaitForSeconds(restDelay); // Rest before next set
        }
    }

    private IEnumerator PerformCharge(Vector2 start, Vector2 end)
    {
        transform.position = start;

        while (Vector2.Distance(transform.position, end) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, end, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        // Deal damage and knock up player if in range
        if (playerRb != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
        {
            playerRb.linearVelocity = new Vector2(Mathf.Sign(end.x - start.x) * 2f, knockUpForce);

            if (playerController != null)
            {
                playerController.enabled = false; // Disable player movement
                Invoke(nameof(EnablePlayerMovement), disableDuration);
            }

            player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void EnablePlayerMovement()
    {
        if (playerController != null)
            playerController.enabled = true;
    }
}
