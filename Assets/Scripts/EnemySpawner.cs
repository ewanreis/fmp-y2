using System;
using UnityEngine;

[Serializable]
public class EnemySpawnData
{
    public GameObject prefab;
    public float spawnChance;
}

public class EnemySpawner : MonoBehaviour
{
    public static event Action<int> OnWaveStart; // called when enemies start spawning
    public static event Action<int> OnStopSpawning; // called when enemies have stop spawning
    public static event Action OnWaveEnd; // called when all spawned enemies are killed

    [SerializeField] private EnemySpawnData[] enemiesToSpawn;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float spawnInterval = 2f; // delay between each enemy spawn
    [SerializeField] private float waveInterval = 5f; // how long to wait between waves
    [SerializeField] private int enemiesPerWave = 10; // how many enemies to spawn

    private Camera mainCamera;
    private Collider2D castleCaptureZone;
    private float nextSpawnTime;
    private int enemiesSpawned;
    private int currentEnemies;
    private int wave;
    private bool isSpawning;

    private void Start()
    {
        mainCamera = Camera.main;
        castleCaptureZone = GameObject.FindWithTag("CastleCaptureZone").GetComponent<Collider2D>();
        OnWaveEnd += StartWave;
    }

    private void Update()
    {
        if (!isSpawning || Time.time < nextSpawnTime || enemiesSpawned >= enemiesPerWave ||
            IsInView() || IsInCastleCaptureZone() || !IsGrounded())
            return;

        SpawnEnemy();
        enemiesSpawned++;
        nextSpawnTime = Time.time + spawnInterval;

        if (enemiesSpawned >= enemiesPerWave)
        {
            isSpawning = false;
            OnStopSpawning?.Invoke(enemiesPerWave);
        }

        if (!isSpawning && currentEnemies == 0)
            OnWaveEnd?.Invoke();
    }

    private bool IsInView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0f && screenPoint.x < 1f && screenPoint.y > 0f && screenPoint.y < 1f;
    }

    private bool IsInCastleCaptureZone() => castleCaptureZone.bounds.Contains(transform.position);

    private bool IsGrounded()
    {
        const float groundRadius = 0.2f;
        int groundLayer = LayerMask.GetMask("Ground");

        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void SpawnEnemy()
    {
        float totalSpawnChance = 0f;
        foreach (EnemySpawnData data in enemiesToSpawn)
            totalSpawnChance += data.spawnChance;

        float spawnRoll = UnityEngine.Random.Range(0f, totalSpawnChance);
        GameObject enemyPrefab = null;

        foreach (EnemySpawnData data in enemiesToSpawn)
        {
            if (spawnRoll < data.spawnChance)
            {
                enemyPrefab = data.prefab;
                break;
            }

            spawnRoll -= data.spawnChance;
        }

        if (enemyPrefab != null)
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    public void StartWave()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            enemiesSpawned = 0;
            OnWaveStart?.Invoke(enemiesPerWave);
        }
    }
}