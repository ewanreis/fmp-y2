using System;
using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    //* This code will spawn enemies in waves across the map outside of the camera bounds

    public static event Action<int> OnWaveStart; // called when enemies start spawning
    public static event Action<int> OnStopSpawning; // called when enemies have stop spawning
    public static event Action OnWaveEnd; // called when all spawned enemies are killed
    public static event Action SurviveFirstYear;
    public static event Action SurviveFifthYear;

    [SerializeField] 
    private EnemySpawnData[] enemiesToSpawn;


    [SerializeField] 
    private Collider2D castleCaptureZone;

    [SerializeField] 
    private Transform enemySpawnCheck;

    [SerializeField] 
    private Transform enemyParent;

    [SerializeField] [Tooltip("Delay between each enemy spawn")] 
    private float spawnInterval = 2f;

    [SerializeField] [Tooltip("How long to wait between waves")] 
    private float waveInterval = 5f;
    
    [SerializeField] [Tooltip("How many enemies to spawn each wave")] 
    private int enemiesPerWave = 10;

    [SerializeField] [Tooltip("The increment of enemies to add next wave")] 
    private int enemyWaveIncrement = 1; 

    [SerializeField]
    private float healthMultiplierIncrement = 0.1f;

    //* The formula for enemies per wave is as follows:
    //* NextSpawnCount = CurrentSpawnCount += (CurrentSpawnCount * EnemyWaveIncrement)

    private Camera mainCamera;
    private float nextSpawnTime;
    private int enemiesSpawned;
    private int currentEnemies;
    private int wave = 0;
    private bool isSpawning;

    private float healthMultiplier = 1;

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
            enemySpawnCheck.transform.position = GenerateRandomPoint(-75, 75, -7.8f, -2.3f);

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
        Vector3 spawnPos = new Vector3(enemySpawnCheck.position.x, enemySpawnCheck.position.y + 5, enemySpawnCheck.position.z);

        if (enemyPrefab != null)
        {
            var enemyGameObject = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemyGameObject.transform.parent = enemyParent;
            Health enemyHealth = enemyGameObject.GetComponentInChildren<Health>();
            enemyHealth.IncreaseMaxHealth(healthMultiplier);
        }
    }

    public void StartWave()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            enemiesSpawned = 0;
            wave++;
            enemiesPerWave += enemyWaveIncrement;
            healthMultiplier += healthMultiplierIncrement;
            if(wave == 2)
                SurviveFirstYear?.Invoke();
            if(wave == 6)
                SurviveFifthYear?.Invoke();
            OnWaveStart?.Invoke(wave);
        }
    }
}

[Serializable]
public class EnemySpawnData
{
    public GameObject prefab;
    public float spawnChance;
}