using UnityEngine;

public abstract class WeaponAttack : MonoBehaviour
{
    protected WeaponInstance weapon;

    public virtual void Initialize(WeaponInstance weapon)
    {
        this.weapon = weapon;
    } 
    
    public abstract bool Attack();
}
