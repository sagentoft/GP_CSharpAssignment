using System;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    public static PooledObjects Instance {get; private set;}
    
    [Header("Object to Pool")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject expPickupPrefab;
    
    List<GameObject> pool_Enemies = new List<GameObject>();
    List<GameObject> pool_projectiles = new List<GameObject>();
    List<GameObject> pool_experiencePickup = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public BaseEnemy GetEnemy(Vector3 location)
    {
        GameObject enemyObject = SpawnObjectFromPool(enemyPrefab, pool_Enemies, location);

        return enemyObject.GetComponent<BaseEnemy>();
    }

    public Projectile GetProjectile(Vector3 location)
    {
        GameObject projectileObject = SpawnObjectFromPool(projectilePrefab, pool_projectiles, location);

        return projectileObject.GetComponent<Projectile>();
    }

    public ExpPickup GetExpPickup(Vector3 location)
    {
        GameObject expPickupObject = SpawnObjectFromPool(expPickupPrefab, pool_experiencePickup, location);
        
        return expPickupObject.GetComponent<ExpPickup>();
    }
    
    private GameObject SpawnObjectFromPool(GameObject objPrefab, List<GameObject> pool, Vector3 location)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].transform.position = location;
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        GameObject obj = Instantiate(objPrefab, location, Quaternion.identity);
        pool.Add(obj); 
        return obj;
    }
}
