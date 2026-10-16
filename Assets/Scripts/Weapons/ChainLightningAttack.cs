using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningAttack : WeaponAttack
{
    [SerializeField] private float jumpRange = 5f;
    [SerializeField] private float jumpDelay = 0.05f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LightningVisual lightningVisualPrefab;
    [SerializeField] private AudioClip lightningSound;

    private bool isAttacking;
    
    public override bool Attack()
    {
        if (isAttacking)
            return false;

        Transform firstTarget = FindClosestTarget(transform.position, weapon.Range, new HashSet<Transform>());

        if (firstTarget == null)
            return false;
        
        AudioManager.Instance.PlaySFX(lightningSound, 0.6f);
        
        StartCoroutine(AttackSequence(firstTarget));
        return true;
    }

    private IEnumerator AttackSequence(Transform firstTarget)
    {
        isAttacking = true;
        
        Vector3 previousPosition = transform.position;
        HashSet<Transform> hitTargets = new();
        Transform currentTarget = firstTarget;

        for (int i = 0; i < weapon.AttackCount; i++)
        {
            if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
                break;
            
            IDamageable<int> damageable = currentTarget.GetComponent<IDamageable<int>>();
            
            Vector3 targetPosition = currentTarget.position;

            LightningVisual visual =
                PooledObjects.Instance.Get<LightningVisual>(
                    lightningVisualPrefab,
                    previousPosition
                );

            visual.Play(previousPosition, targetPosition);

            previousPosition = targetPosition;
            if (damageable != null)
                damageable.Damage(weapon.Damage);
            
            hitTargets.Add(currentTarget);

            if (i == weapon.AttackCount - 1)
                break;

            Vector3 jumpOrigin = currentTarget.position;

            currentTarget = FindClosestTarget(jumpOrigin, jumpRange, hitTargets);

            if (currentTarget == null)
                break;
            
            yield return new WaitForSeconds(jumpDelay);
        }
        
        isAttacking = false;
    }

    private Transform FindClosestTarget(Vector3 origin, float radius, HashSet<Transform> excludedTargets)
    {
        Collider[] enemies = Physics.OverlapSphere(origin, radius, enemyLayer);

        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider enemy in enemies)
        {
            Transform target = enemy.transform;

            if (excludedTargets.Contains(target))
                continue;
            
            float distance = (target.position - origin).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }
}
