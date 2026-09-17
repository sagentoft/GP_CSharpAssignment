using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //[SerializeField] private PooledObjects pooledObjects;
    
    [SerializeField] private float range = 10f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private LayerMask enemyLayer;

    private float nextAttackTime;

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Collider target = FindClosestEnemy();
            
            if (target != null)
            {
                Attack(target);
                
                nextAttackTime = Time.time + attackInterval;
            }
        }
    }

    private Collider FindClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(
            transform.position,
            range,
            enemyLayer
        );

        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (Collider col in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = col;
                closestDistance = distance;
            }
        }
        
        return closestEnemy;
    }

    private void Attack(Collider target)
    {
        Vector3 targetDirection = target.transform.position - transform.position;

        Projectile projectile = PooledObjects.Instance.GetProjectile(transform.position);
        
        projectile.Initialize(targetDirection);
    }
}
