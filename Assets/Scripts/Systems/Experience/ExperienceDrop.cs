using System;
using UnityEngine;

public class ExperienceDrop : MonoBehaviour
{
    [SerializeField] private ExpPickup expPickupPrefab;
    
    private BaseEnemy enemy;
    private int expValue;

    private void Awake()
    {
        enemy = GetComponent<BaseEnemy>();
    }

    private void OnEnable()
    {
        enemy.OnDeath += DropExperience;
    }

    private void OnDisable()
    {
        enemy.OnDeath -= DropExperience;
    }

    private void DropExperience()
    {
        ExpPickup pickup = PooledObjects.Instance.Get(expPickupPrefab, transform.position);
        pickup.Initialize(enemy.ExperienceReward);
    }
}
