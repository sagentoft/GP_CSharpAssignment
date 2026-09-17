using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    //[SerializeField] private PooledObjects pool;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnDistance = 15f;

    private float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        
        Vector3 direction = new Vector3(
            randomDirection.x,
            0,
            randomDirection.y
        );
        
        Vector3 spawnPosition = player.position + direction * spawnDistance;

        BaseEnemy enemy = PooledObjects.Instance.GetEnemy(spawnPosition);
        enemy.SetTarget(player);
    }
}
