using System;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    public static PooledObjects Instance {get; private set;}
    
    private Dictionary<GameObject, List<GameObject>> pools = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public T Get<T>(T prefab, Vector3 location) where T : Component
    {
        GameObject obj = SpawnObjectFromPool(prefab.gameObject, location);

        return obj.GetComponent<T>();
    }

    private GameObject SpawnObjectFromPool(GameObject objPrefab, Vector3 location)
    {
        if (!pools.TryGetValue(objPrefab, out List<GameObject> pool))
        {
            pool = new List<GameObject>();
            pools.Add(objPrefab, pool);
        }

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
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
