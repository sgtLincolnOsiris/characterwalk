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

    [Header("Doors Settings")]
    public MissionDoor finalDoor;
    public MissionDoor deathDoor;

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
        if (finalDoor != null) finalDoor.LockDoor();
        if (deathDoor != null) deathDoor.LockDoor();
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
                yield return null;
                UpdateUI();
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
        if (!missionActive) return;

        missionActive = false;
        if (deathDoor != null) deathDoor.UnlockDoor();
        Debug.Log("Player Died! Death Door Opened!");
    }

    void OpenFinalDoor()
    {
        missionActive = false;
        if (finalDoor != null) finalDoor.UnlockDoor();
        Debug.Log("Final Door Opened!");
    }

    void UpdateUI()
    {
        if (waveText != null) waveText.text = "Wave: " + currentWave + " / " + totalWaves;
        if (enemiesAliveText != null) enemiesAliveText.text = "Enemies Alive: " + (enemiesToKill - enemiesKilled);
    }
}

[System.Serializable]
public class MissionDoor
{
    public GameObject targetDoor;
    public AudioClip triggerSound;

    private AudioSource audioSource;

    public void LockDoor()
    {
        if (targetDoor) targetDoor.SetActive(false);
    }

    public void UnlockDoor()
    {
        if (targetDoor) targetDoor.SetActive(true);
        PlaySound();
    }

    private void PlaySound()
    {
        if (triggerSound == null) return;

        if (audioSource == null)
        {
            audioSource = targetDoor.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        audioSource.PlayOneShot(triggerSound);
    }
}
