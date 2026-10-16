using UnityEngine;

[System.Serializable]
public class WeaponCooldownEffect : WeaponUpgradeEffect
{
    [SerializeField] private float multiplier = 1f;


    protected override void ApplyToWeapon(WeaponInstance weapon)
    {
        weapon.ModifyCooldown(multiplier);
    }
}
