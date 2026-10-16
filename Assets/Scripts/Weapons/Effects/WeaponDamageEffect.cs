using System;
using UnityEngine;

[Serializable]
public class WeaponDamageEffect : WeaponUpgradeEffect
{
    
    [SerializeField] private float multiplier = 1.0f;


    protected override void ApplyToWeapon(WeaponInstance weapon)
    {
        weapon.ModifyDamage(multiplier);
    }
}
