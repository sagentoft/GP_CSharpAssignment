using UnityEngine;

public interface IDamageable<T>
{
    void Damage(T damage);
}
