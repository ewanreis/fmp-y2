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
    [SerializeField] private Collider2D castleCaptureZone;
    [SerializeField] private Transform enemySpawnCheck;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float spawnInterval = 2f; // delay between each enemy spawn
    [SerializeField] private float waveInterval = 5f; // how long to wait between waves
    [SerializeField] private int enemiesPerWave = 10; // how many enemies to spawn
    [SerializeField] private double enemyWaveIncrement = 0.1; // the increment of enemies to add next wave

    //* The formula for enemies per wave is as follows:
    //* NextSpawnCount = CurrentSpawnCount += (CurrentSpawnCount * EnemyWaveIncrement)

    private Camera mainCamera;
    private float nextSpawnTime;
    private int enemiesSpawned;
    private int currentEnemies;
    private int wave = 0;
    private bool isSpawning;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        //castleCaptureZone = GameObject.FindWithTag("CastleCaptureZone").GetComponent<Collider2D>();
        OnWaveEnd += StartWave;
        StartWave();
    }

    private void OnDisable() 
    {
        OnWaveEnd -= StartWave;
    }

    private void Update()
    {
        if(isSpawning)
            enemySpawnCheck.transform.position = GenerateRandomPoint(-100, 100, -7.8f, -2.3f);

        if (Time.time >= nextSpawnTime && !IsInView() && !IsInCastleCaptureZone() && IsGrounded() && isSpawning)
        {
            SpawnEnemy();
            enemiesSpawned++;
            nextSpawnTime = Time.time + spawnInterval;
        }

        currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemiesSpawned >= enemiesPerWave || currentEnemies >= enemiesPerWave)
        {
            isSpawning = false;
            OnStopSpawning?.Invoke(enemiesPerWave);
        }

        if (!isSpawning && currentEnemies == 0)
            OnWaveEnd?.Invoke();
    }

    private Vector2 GenerateRandomPoint(float minX, float maxX, float minY, float maxY)
    {
        return new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
    }

    private bool IsInView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(enemySpawnCheck.position);
        return screenPoint.x > 0f && screenPoint.x < 1f && screenPoint.y > 0f && screenPoint.y < 1f;
    }

    private bool IsInCastleCaptureZone() => castleCaptureZone.bounds.Contains(enemySpawnCheck.position);

    private bool IsGrounded()
    {
        const float groundRadius = 0.2f;
        int groundLayer = LayerMask.GetMask("Ground");
        return Physics2D.OverlapCircle(enemySpawnCheck.position, groundRadius, groundLayer);
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
        Vector3 spawnPos = new Vector3(enemySpawnCheck.position.x, enemySpawnCheck.position.y + 3, enemySpawnCheck.position.z);

        if (enemyPrefab != null)
        {
            var enemyGameObject = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemyGameObject.transform.parent = enemyParent;
        }
    }

    public void StartWave()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            enemiesSpawned = 0;
            wave++;
            OnWaveStart?.Invoke(wave);
        }
    }
}