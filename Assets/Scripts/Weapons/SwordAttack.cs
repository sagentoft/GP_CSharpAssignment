using System;
using System.Collections;
using UnityEngine;

public class SwordAttack : WeaponAttack
{
    [SerializeField] private SwordSwing swingPrefab;
    [SerializeField] private float attackOffset = 1.5f;
    [SerializeField] private float attackDelay = 0.15f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioClip swingSound;

    private PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public override bool Attack()
    {

        if (GetEnemiesInRange().Length == 0)
            return false;

        StartCoroutine(AttackSequence());

        return true;
    }

    private Collider[] GetEnemiesInRange()
    {
        Vector3 attackCenter = transform.position + player.FacingDirection * attackOffset;
        
        return Physics.OverlapSphere(attackCenter, weapon.Range, enemyLayer);
    }

    private void PerformSwing(WeaponInstance weapon, int swingIndex)
    {
        AudioManager.Instance.PlaySFX(swingSound, 0.5f);
        
        foreach (Collider enemy in GetEnemiesInRange())
        {
            IDamageable<int> damageable = enemy.GetComponent<IDamageable<int>>();

            if (damageable != null)
            {
                damageable.Damage(weapon.Damage);
            }
        }
        
        PlaySwingVisual(swingIndex);
    }

    private IEnumerator AttackSequence()
    {
        for (int i = 0; i < weapon.AttackCount; i++)
        {
            PerformSwing(weapon, i);

            if (i < weapon.AttackCount - 1)
            {
                yield return new WaitForSeconds(attackDelay);
            }
        }
    }
    
    private void PlaySwingVisual(int swingIndex)
    {
        SwordSwing swing =
            PooledObjects.Instance.Get<SwordSwing>(
                swingPrefab,
                player.transform.position
            );

        swing.Play(player.FacingDirection, weapon.Range, swingIndex % 2 != 0);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        Vector3 attackCenter =
            transform.position + player.FacingDirection * attackOffset;

        //Gizmos.DrawWireSphere(attackCenter, range);
    }
}
