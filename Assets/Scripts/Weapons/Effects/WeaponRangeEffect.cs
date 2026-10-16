using UnityEngine;

[System.Serializable]
public class WeaponRangeEffect : WeaponUpgradeEffect
{
    
    [SerializeField] private float rangeMultiplier = 1f;


    protected override void ApplyToWeapon(WeaponInstance weapon)
    {
        weapon.ModifyRange(rangeMultiplier);
    }
}
