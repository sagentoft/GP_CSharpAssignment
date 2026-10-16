using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private List<Weapon> weapons = new();
    
    public void AddWeapon(WeaponData weaponData)
    {
        Weapon weapon = Instantiate(weaponData.WeaponPrefab, transform);

        weapon.Initialize(weaponData);
        
        weapons.Add(weapon);
    }

    public WeaponInstance GetWeapon(WeaponData weaponData)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.Instance.Data == weaponData)
            {
                return weapon.Instance;
            }
        }
        
        return null;
    }
}
