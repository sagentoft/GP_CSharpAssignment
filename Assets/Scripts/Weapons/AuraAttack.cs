using System;
using UnityEngine;

public class AuraAttack : WeaponAttack
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform auraVisual;
    
    public override bool Attack()
    {
        
        Collider[] enemies = Physics.OverlapSphere(transform.position, weapon.Range, enemyLayer);

        if (enemies.Length == 0)
            return false;

        foreach (Collider enemy in enemies)
        {
            IDamageable<int> damageable = enemy.GetComponent<IDamageable<int>>();

            if (damageable != null)
            {
                damageable.Damage(weapon.Damage);
            }
        }
        
        return true;
    }

    private void Update()
    {
        if (weapon == null)
            return;

        float diameter = weapon.Range * 2;
        
        auraVisual.localScale = new Vector3(diameter, auraVisual.localScale.y, diameter);
    }
}
