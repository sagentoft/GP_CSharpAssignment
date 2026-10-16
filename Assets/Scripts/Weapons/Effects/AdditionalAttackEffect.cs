using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AdditionalAttackEffect : WeaponUpgradeEffect
{
    
    [FormerlySerializedAs("projectilesToAdd")] [SerializeField] private int attacksToAdd;


    protected override void ApplyToWeapon(WeaponInstance weapon)
    {
        weapon.AddAttacks(attacksToAdd);
    }
}
