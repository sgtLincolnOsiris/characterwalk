using System.Collections;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss;
    public Transform bossStartPosition;
    public Transform bossFightPosition;
    public float moveSpeed = 2f;
    public float roarDelay = 1f;
    public Animator bossAnimator;
    public AudioSource bossAudioSource;
    public AudioClip roarSound;

    private bool playerInTrigger = false;
    private bool bossReady = false;

    private GameObject player;
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerMovement = player.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
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
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, bossFightPosition.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(boss.transform.position, bossFightPosition.position) < 0.1f)
            {
                playerInTrigger = false;
                StartCoroutine(BossRoarAndStartFight());
            }
        }
    }

    IEnumerator BossRoarAndStartFight()
    {
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Roar");
        }

        if (bossAudioSource != null && roarSound != null)
        {
            bossAudioSource.PlayOneShot(roarSound);
        }

        yield return new WaitForSeconds(roarDelay);

        Debug.Log("Boss is ready to fight!");
        bossReady = true; // You need to set this flag to true so the boss is ready to fight.
    }

    private void FlipBoss(float playerPositionX)
    {
        if (playerPositionX > boss.transform.position.x)
        {
            boss.transform.localScale = new Vector3(1, 1, 1); // Make boss face right
        }
        else
        {
            boss.transform.localScale = new Vector3(-1, 1, 1); // Make boss face left
        }
    }
}
