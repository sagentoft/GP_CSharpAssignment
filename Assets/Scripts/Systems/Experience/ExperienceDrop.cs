using System;
using UnityEngine;

public class ExperienceDrop : MonoBehaviour
{
    private BaseEnemy enemy;

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
        PooledObjects.Instance.GetExpPickup(transform.position);
    }
}
