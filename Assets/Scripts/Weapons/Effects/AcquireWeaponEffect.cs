using System;
using UnityEngine;

[Serializable]
public class AcquireWeaponEffect : UpgradeEffect
{
    public override void Apply(GameObject target, UpgradeData upgrade)
    {
        WeaponManager weaponManager = target.GetComponent<WeaponManager>();

        if (weaponManager == null || upgrade.WeaponData == null)
            return;
        
        weaponManager.AddWeapon(upgrade.WeaponData);
    }
}
