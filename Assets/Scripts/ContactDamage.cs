using System;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 1f;
    
    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= nextDamageTime)
        {
            IDamageable<int> damageable = other.GetComponent<IDamageable<int>>();

            if (damageable != null)
            {
                damageable.Damage(damage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}
