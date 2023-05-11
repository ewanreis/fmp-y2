using UnityEngine;

public class SlimeShooter : MonoBehaviour
{
    //* This script spawns slimes from the sky at a fixed interval which shoot towards the castle
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float shootForce = 50f;
    [SerializeField] private Transform spawnParent;

    private float timeSinceLastSpawn;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

     private void SpawnEnemy()
    {
        float spawnAngle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 spawnPos = new Vector2(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle)) * spawnRadius;
        RaycastHit2D hit = Physics2D.Raycast(spawnPos, Vector2.down);

        if (hit.collider != null)
        {
            float ySpawnPos = hit.point.y + 20;
            spawnPos.y = ySpawnPos;
        }

        else
        {
            spawnPos.y = 20f;
        }
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.parent = spawnParent;

        Vector2 shootDirection = ((Vector2.zero - spawnPos).normalized + Random.insideUnitCircle * 0.1f).normalized;
        Quaternion shootRotation = Quaternion.LookRotation(Vector3.forward, shootDirection);

        enemy.transform.rotation = shootRotation;

        Rigidbody2D rb = enemy.GetComponentInChildren<Rigidbody2D>();
        ExplodeOnCollision explodeOnCollision = enemy.GetComponentInChildren<ExplodeOnCollision>();
        explodeOnCollision.StartExplosion(4);

        rb.AddForce(shootDirection * shootForce, ForceMode2D.Impulse);
    }
}
