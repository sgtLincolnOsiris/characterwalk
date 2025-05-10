using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss; // The Boss object
    public Transform bossStartPosition; // Boss starting position
    public Transform bossFightPosition; // Position where the boss should move to for the fight
    public float moveSpeed = 2f; // Speed at which the boss moves
    public float roarDelay = 1f; // Delay before the boss roars
    public Animator bossAnimator; // Animator for the boss
    public AudioSource bossAudioSource; // AudioSource for the boss
    public AudioClip roarSound; // Roar sound for the boss

    private bool playerInTrigger = false;
    private bool bossReady = false;

    private GameObject player; // Reference to player
    private PlayerController playerController; // Reference to player controller

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerController = player.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.enabled = false; // Disable player movement
            }

            playerInTrigger = true;
            boss.SetActive(true);
            FlipBoss(player.transform.position.x);
        }
    }

    private void Update()
    {
        if (playerInTrigger && !bossReady)
        {
            // Move the boss towards the fight position
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, bossFightPosition.position, moveSpeed * Time.deltaTime);

            // Check if the boss reached the fight position
            if (Vector2.Distance(boss.transform.position, bossFightPosition.position) < 0.1f)
            {
                playerInTrigger = false;
                StartCoroutine(BossRoarAndStartFight());
            }
        }
    }

    IEnumerator BossRoarAndStartFight()
    {
        // Trigger roar animation
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Roar");
        }

        // Play roar sound
        if (bossAudioSource != null && roarSound != null)
        {
            bossAudioSource.PlayOneShot(roarSound);
        }

        yield return new WaitForSeconds(roarDelay);

        Debug.Log("Boss is ready to fight!");
        bossReady = true; // The fight starts

        if (playerController != null)
        {
            playerController.enabled = true; // Re-enable player movement
        }
    }

    void FlipBoss(float playerX)
    {
        Vector3 bossScale = boss.transform.localScale;

        // Flip based on player position
        if (playerX > boss.transform.position.x)
            bossScale.x = Mathf.Abs(bossScale.x); // Face right
        else
            bossScale.x = -Mathf.Abs(bossScale.x); // Face left

        boss.transform.localScale = bossScale;
    }
}
