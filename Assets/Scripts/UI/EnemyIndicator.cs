using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject arrowPrefab;
    public int enemiesLeftThreshold = 3;

    public List<Transform> enemies;
    public List<GameObject> arrows;

    [SerializeField] private Transform arrowParent;

    private void OnEnable()
    {
        Health.OnDeath += FindEnemies;
    }

    private void OnDisable()
    {
        Health.OnDeath -= FindEnemies;
    }

    private void FindEnemies(int _) => FindEnemies();
    private void FindEnemies(Creature _, bool b) => FindEnemies();

    private void FindEnemies()
    {
        Debug.Log("find");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemyObjects.Length);
        if(enemyObjects.Length > enemiesLeftThreshold)
            return;

        Debug.Log("continue");
        foreach (GameObject arrow in arrows)
            Destroy(arrow.gameObject);

        enemies = new List<Transform>();
        arrows = new List<GameObject>();


        foreach (GameObject enemyObject in enemyObjects)
        {
            enemies.Add(enemyObject.transform);
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.SetParent(arrowParent, false);
            arrows.Add(arrow);
        }
    }

    private void FixedUpdate()
{
    if (enemies.Count == 0)
        return;

    int enemiesLeft = 0;

    for (int i = 0; i < enemies.Count; i++)
    {
        if (enemies[i] == null)
        {
            enemies.Remove(enemies[i]);
            Destroy(arrows[i].gameObject);
            arrows.Remove(arrows[i]);
            continue;
        }

        Vector3 enemyScreenPos = Camera.main.WorldToScreenPoint(enemies[i].position);
        bool isOffScreen = enemyScreenPos.x < 0 || enemyScreenPos.x > Screen.width ||
            enemyScreenPos.y < 0 || enemyScreenPos.y > Screen.height;

        if (isOffScreen)
        {
            arrows[i].SetActive(true);
            Vector3 arrowScreenPos = enemyScreenPos;
            arrowScreenPos.x = Mathf.Clamp(arrowScreenPos.x, 40f, Screen.width - 40f);
            arrowScreenPos.y = Mathf.Clamp(arrowScreenPos.y, 40f, Screen.height - 40f);
            arrows[i].transform.position = arrowScreenPos;

            Vector3 enemyScreenPos2D = new Vector3(enemyScreenPos.x, enemyScreenPos.y, 0f);
            Vector3 playerScreenPos2D = new Vector3(playerTransform.position.x, playerTransform.position.y, 0f);
            Vector3 arrowDir2D = enemyScreenPos2D - playerScreenPos2D;
            arrows[i].transform.up = arrowDir2D.normalized;
        }
        else
        {
            arrows[i].SetActive(false);
        }
    }
}
}
