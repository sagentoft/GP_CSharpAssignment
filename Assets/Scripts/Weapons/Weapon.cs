using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponAttack attackBehavior;

    private WeaponInstance instance;
    private float nextAttackTime;
    
    public WeaponInstance Instance => instance;
    

    public void Initialize(WeaponData weaponData)
    {
        instance = new WeaponInstance(weaponData);
        attackBehavior.Initialize(instance);
    }

    private void Update()
    {
        if (instance == null)
            return;
        
        if (Time.time < nextAttackTime)
            return;

        if (attackBehavior.Attack())
        {
            nextAttackTime = Time.time + instance.Cooldown;
        }
    }
}
