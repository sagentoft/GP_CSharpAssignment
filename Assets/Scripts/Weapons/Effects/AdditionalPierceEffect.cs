using UnityEngine;

[System.Serializable]
public class AdditionalPierceEffect : WeaponUpgradeEffect
{
    [SerializeField] private int amount = 1;

    protected override void ApplyToWeapon(WeaponInstance weapon)
    {
        weapon.AddPierce(amount);
    }
}
