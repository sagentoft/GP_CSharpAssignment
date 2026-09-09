using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    [Header("Object to Pool")] 
    [SerializeField] private GameObject enemyPrefab;
    
    List<GameObject> pool_Enemies = new List<GameObject>();

    public BaseEnemy GetEnemy(Vector3 location)
    {
        GameObject enemyObject = SpawnObjectFromPool(enemyPrefab, pool_Enemies, location);

        return enemyObject.GetComponent<BaseEnemy>();
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
