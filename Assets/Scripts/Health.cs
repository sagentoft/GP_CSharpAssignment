using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable<int>
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int minHealth;
    [SerializeField] private int currentHealth;

    private int defaultHealth = 50;

    private IKillable owner;


    private void Awake()
    {
        owner = GetComponent<IKillable>();
        maxHealth = SetDefaultHealth(maxHealth);
        
        if (owner == null)
        {
            Debug.LogError($"{name} has a Health component but does not implement IKillable.");
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private int SetDefaultHealth(int currentMaxVal)
    {
        if (currentMaxVal > 0)
        {
            return currentMaxVal;
        }
        else
        {
            return defaultHealth;
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= minHealth)
        {
            owner.Die();
        }
    }
}
