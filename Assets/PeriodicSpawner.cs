using UnityEngine;

public class PeriodicSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject prefabToSpawn;
    public float spawnInterval = 2f;
    public int maxSpawns = 5;

    private float spawnTimer;
    private int currentSpawns;

    void Update()
    {
        if (!gameObject.activeSelf) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && currentSpawns < maxSpawns)
        {
            SpawnObject();
            spawnTimer = 0f;
            currentSpawns++;
        }
    }

    void SpawnObject()
    {
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }

    // Reset when disabled
    private void OnDisable()
    {
        spawnTimer = 0f;
        currentSpawns = 0;
    }
}