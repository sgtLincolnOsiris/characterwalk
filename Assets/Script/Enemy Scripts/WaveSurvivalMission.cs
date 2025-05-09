using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSurvivalMission : MonoBehaviour
{
    [Header("Mission Settings")]
    public Transform player;
    public List<Transform> enemySpawnPoints;
    public GameObject enemyType1Prefab; // Melee
    public GameObject enemyType2Prefab; // Ranged
    public GameObject finalDoor;
    public GameObject deathDoor;

    [Header("UI Settings")]
    public Text waveText;
    public Text enemiesAliveText;

    public int totalWaves = 3;

    private int currentWave = 0;
    private int enemiesToKill = 5;
    private int enemiesKilled = 0;
    private bool missionActive = false;
    private bool missionTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !missionActive && !missionTriggered)
        {
            missionTriggered = true;
            StartMission();
        }
    }

    void StartMission()
    {
        missionActive = true;
        currentWave = 0;
        finalDoor.SetActive(false);
        deathDoor.SetActive(false);
        UpdateUI();
        StartCoroutine(WaveManager());
    }

    IEnumerator WaveManager()
    {
        for (int wave = 0; wave < totalWaves; wave++)
        {
            currentWave++;
            enemiesKilled = 0;
            SpawnEnemies();
            UpdateUI();

            while (enemiesKilled < enemiesToKill)
            {
                UpdateUI();
                yield return null;
            }
        }

        OpenFinalDoor();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < Mathf.Min(enemySpawnPoints.Count, enemiesToKill); i++)
        {
            GameObject enemy = (enemySpawnPoints[i].position.y > 0) ? enemyType2Prefab : enemyType1Prefab;
            Instantiate(enemy, enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
        }
    }

    public void EnemyDefeated()
    {
        enemiesKilled++;
        UpdateUI();
    }

    public void PlayerDied()
    {
        if (missionActive)
        {
            deathDoor.SetActive(true);
            missionActive = false;
            Debug.Log("Player Died! Death Door Opened!");
        }
    }

    void OpenFinalDoor()
    {
        missionActive = false;
        finalDoor.SetActive(true);
        Debug.Log("Final Door Opened!");
    }

    void UpdateUI()
    {
        waveText.text = "Wave: " + currentWave + " / " + totalWaves;
        enemiesAliveText.text = "Enemies Alive: " + (enemiesToKill - enemiesKilled);
    }
}
