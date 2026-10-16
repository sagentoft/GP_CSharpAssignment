using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileAttack : WeaponAttack
{
    [SerializeField] private Projectile projectilePrefab;
    
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float range = 10f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioClip castSound;

    private bool isAttacking;
    
    public override bool Attack()
    {
        if (isAttacking)
            return false;
        
        List<Transform> targets = GetClosestEnemies(weapon.AttackCount);

        if (targets.Count == 0)
            return false;

        StartCoroutine(AttackSequence(weapon, targets));
        
        return true;
    }

    private List<Transform> GetClosestEnemies(int count)
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);

        List<Transform> targets = new();

        foreach (Collider enemy in enemies)
        {
            targets.Add(enemy.transform);
        }

        targets.Sort((a, b) =>
        {
            float distanceA = (a.position - transform.position).sqrMagnitude;
            float distanceB = (b.position - transform.position).sqrMagnitude;

            return distanceA.CompareTo(distanceB);
        });

        if (targets.Count == 0)
            return targets;

        if (targets.Count > count)
        {
            targets.RemoveRange(count, targets.Count - count);
        }
    
        int uniqueTargetCount = targets.Count;

        for (int i = uniqueTargetCount; i < count; i++)
        {
            targets.Add(targets[i % uniqueTargetCount]);
        }

        return targets;
    }

    private IEnumerator AttackSequence(WeaponInstance weapon, List<Transform> targets)
    {
        isAttacking = true;
        
        foreach (Transform target in targets)
        {
            FireProjectile(weapon, target);
            AudioManager.Instance.PlaySFX(castSound, 0.35f);
            
            yield return new WaitForSeconds(attackDelay);
            
        }
        
        isAttacking = false;
    }

    private void FireProjectile(WeaponInstance weapon, Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Projectile projectile = PooledObjects.Instance.Get<Projectile>(projectilePrefab, transform.position);
        
        projectile.Initialize(direction, projectileSpeed, weapon.Damage, weapon.Pierce);
    }
}
