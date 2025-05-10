using System.Collections;
using UnityEngine;

public class BossChargeBackAndForth : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public float chargeSpeed = 10f;
    public float knockUpForce = 5f;
    public float disableDuration = 0.5f;
    public int damage = 20;

    public Transform pointA;
    public Transform pointB;
    public int chargeCount = 3;
    public float restDelay = 2f;

    private Rigidbody2D playerRb;
    private PlayerMovement playerMovement; // CHANGED from PlayerController

    private void Start()
    {
        player = GameManager.Instance.player;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>(); // CHANGED

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

            yield return new WaitForSeconds(restDelay);
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

        if (playerRb != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
        {
            playerRb.linearVelocity = new Vector2(Mathf.Sign(end.x - start.x) * 2f, knockUpForce);

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
                Invoke(nameof(EnablePlayerMovement), disableDuration);
            }

            player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void EnablePlayerMovement()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}
