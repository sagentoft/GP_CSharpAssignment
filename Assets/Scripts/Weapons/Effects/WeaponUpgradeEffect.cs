using System;
using UnityEngine;

[Serializable]
public abstract class WeaponUpgradeEffect : UpgradeEffect
{
    public sealed override void Apply(GameObject target, UpgradeData upgrade)
    {
        WeaponManager weaponManager =
            target.GetComponent<WeaponManager>();

        if (weaponManager == null)
            return;

        WeaponInstance weapon =
            weaponManager.GetWeapon(upgrade.WeaponData);

        if (weapon == null)
            return;
        
        ApplyToWeapon(weapon);
    }

    protected abstract void ApplyToWeapon(WeaponInstance weapon);
}
