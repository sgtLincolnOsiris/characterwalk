// WaveManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int totalWaves = 3;
    public int enemiesPerSpawnPoint = 5; // Changed from enemiesPerWave
    public float timeBetweenWaves = 10f;

    [Header("Enemy Types")]
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    [Range(0f, 1f)] public float rangedEnemyChance = 0.3f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Assign these in inspector

    [Header("Completion")]
    public GameObject objectToActivateOnSuccess;
    public AudioClip successSound;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private bool missionInProgress = false;
    private bool playerAlive = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void StartMission()
    {
        if (missionInProgress) return;

        missionInProgress = true;
        playerAlive = true;
        currentWave = 0;
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (!playerAlive || spawnPoints.Length == 0) return;

        currentWave++;
        enemiesRemaining = enemiesPerSpawnPoint * spawnPoints.Length;

        Debug.Log($"Starting Wave {currentWave}");

        // Spawn enemies at each spawn point
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemiesPerSpawnPoint; i++)
            {
                SpawnEnemy(spawnPoint.position);
            }
        }
    }

    private void SpawnEnemy(Vector2 position)
    {
        // Decide which enemy to spawn
        GameObject enemyPrefab = Random.value <= rangedEnemyChance ? rangedEnemyPrefab : meleeEnemyPrefab;

        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    public void EnemyDefeated()
    {
        if (!missionInProgress) return;

        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            if (currentWave >= totalWaves)
            {
                MissionComplete();
            }
            else
            {
                Invoke("StartNextWave", timeBetweenWaves);
            }
        }
    }

    public void PlayerDied()
    {
        playerAlive = false;
        missionInProgress = false;

        // Reset mission progress
        Debug.Log("Mission Failed - Player Died");

        // Clean up remaining enemies
        CleanUpEnemies();
    }

    private void MissionComplete()
    {
        missionInProgress = false;
        Debug.Log("Mission Complete!");

        // Activate the success object
        if (objectToActivateOnSuccess != null)
        {
            objectToActivateOnSuccess.SetActive(false);
        }

        // Play success sound
        if (successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }
    }

    private void CleanUpEnemies()
    {
        EnemyAI[] meleeEnemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in meleeEnemies)
        {
            Destroy(enemy.gameObject);
        }

        RangedEnemyAI[] rangedEnemies = FindObjectsOfType<RangedEnemyAI>();
        foreach (RangedEnemyAI enemy in rangedEnemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    // Call this from a UI button or trigger to start the mission
    public void OnMissionStartButton()
    {
        StartMission();
    }
}