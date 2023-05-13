using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] soldierPrefabs;
    [SerializeField] private GameObject[] spawnEffectPrefabs;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float spawnForce = 5f;

    public void SpawnObject(int soldierIndex)
    {
        GameObject spawnEffect = Instantiate(spawnEffectPrefabs[soldierIndex], spawnPoint.position, Quaternion.identity);
        GameObject spawnedObject = Instantiate(soldierPrefabs[soldierIndex], spawnPoint.position, Quaternion.identity);
        Destroy(spawnEffect, 5f);

        // upwards force when spawning
        Rigidbody2D spawnedRigidbody = spawnedObject.GetComponent<Rigidbody2D>();
        if (spawnedRigidbody != null)
            spawnedRigidbody.AddForce(Vector2.up * spawnForce, ForceMode2D.Impulse);
    }
}
