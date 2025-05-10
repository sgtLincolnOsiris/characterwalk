using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPounce : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public float pounceSpeed = 15f; // Speed of the pounce
    public float knockBackForce = 3f; // Force of knockback
    public float disableDuration = 0.5f; // Duration to disable player movement
    public int damage = 20; // Damage dealt by the pounce

    public int pounceCount = 3; // Number of pounces in one set
    public float intervalBetweenPounces = 1f; // Interval between each pounce
    public float restDelay = 2f; // Delay before next set of pounces

    private Rigidbody2D playerRb;
    private PlayerController playerController;

    private void Start()
    {
        player = GameManager.Instance.player; // Get player from GameManager
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();

        StartCoroutine(PounceSequence());
    }

    private IEnumerator PounceSequence()
    {
        while (true)
        {
            for (int i = 0; i < pounceCount; i++)
            {
                yield return StartCoroutine(PounceAtPlayer());
                yield return new WaitForSeconds(intervalBetweenPounces);
            }

            yield return new WaitForSeconds(restDelay);
        }
    }

    private IEnumerator PounceAtPlayer()
    {
        Vector2 targetPosition = player.transform.position;

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, pounceSpeed * Time.deltaTime);
            yield return null;
        }

        // Deal damage and knockback player
        if (playerRb != null)
        {
            Vector2 knockDirection = (player.transform.position - transform.position).normalized * -knockBackForce;
            playerRb.linearVelocity = knockDirection;

            if (playerController != null)
            {
                playerController.enabled = false;
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
