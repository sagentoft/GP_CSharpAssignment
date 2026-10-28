using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnEntry> enemyTypes;

    [SerializeField] private Transform player;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float stageDuration = 120f;
    [SerializeField] private int additionalEnemiesPerStage = 1;
    [SerializeField] private float spawnDistance = 15f;

    private float spawnTimer;
    private float elapsedTime;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        int stage = Mathf.FloorToInt(elapsedTime / stageDuration);
        
        int spawnCount = 1 + stage * additionalEnemiesPerStage;

        if (spawnTimer >= spawnInterval)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
            }
            
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        BaseEnemy selectedPrefab = SelectEnemyPrefab();

        if (selectedPrefab == null)
            return;
        
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        
        Vector3 direction = new Vector3(
            randomDirection.x,
            0,
            randomDirection.y
        );
        
        Vector3 spawnPosition = player.position + direction * spawnDistance;

        BaseEnemy enemy = PooledObjects.Instance.Get(selectedPrefab, spawnPosition);
        enemy.SetTarget(player);
    }

    private BaseEnemy SelectEnemyPrefab()
    {
        int totalWeight = 0;

        foreach (var entry in enemyTypes)
        {
            if (entry.prefab != null)
                totalWeight += GetWeight(entry);
        }

        if (totalWeight <= 0)
            return null;

        int roll = Random.Range(0, totalWeight);

        foreach (var entry in enemyTypes)
        {
            if (elapsedTime < entry.unlockTime || entry.prefab == null)
                continue;
            
            int weight = GetWeight(entry);
            
            if (roll < weight)
                return entry.prefab;
            
            roll -= weight;
        }
        
        return null;
    }

    private int GetWeight(EnemySpawnEntry entry)
    {
        if (elapsedTime < entry.unlockTime)
            return 0;

        if (entry.transitionDuration <= 0f)
            return Mathf.Max(0, entry.finalWeight);

        float timeSinceUnlock = elapsedTime - entry.unlockTime;

        float progress = Mathf.Clamp01(timeSinceUnlock / entry.transitionDuration);
        
        return Mathf.Max(0, Mathf.RoundToInt(Mathf.Lerp(entry.startingWeight, entry.finalWeight, progress)));
    }
}
