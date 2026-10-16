using UnityEngine;

public class WeaponInstance
{
    private WeaponData data;

    private int damage;
    private float cooldown;

    public int AttackCount { get; private set; } = 1;
    public float Range { get; private set; }
    public int Pierce { get; private set; }
    
    public WeaponData Data => data;
    
    public int Damage => damage;
    public float Cooldown => cooldown;

    public WeaponInstance(WeaponData data)
    {
        this.data = data;

        damage = data.BaseDamage;
        cooldown = data.BaseCooldown;
        Range = data.BaseRange;
    }

    public void ModifyDamage(float multiplier)
    {
        damage = Mathf.RoundToInt(damage * multiplier);
    }

    public void ModifyCooldown(float multiplier)
    {
        cooldown *= multiplier;
    }

    public void AddAttacks(int amount)
    {
        AttackCount += amount;
    }

    public void ModifyRange(float multiplier)
    {
        Range *= multiplier;
    }

    public void AddPierce(int amount)
    {
        Pierce = Mathf.Max(0, Pierce + amount);
    }
    
}
