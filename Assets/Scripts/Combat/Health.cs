using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable<int>
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int minHealth;
    [SerializeField] private int currentHealth;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private int defaultHealth = 50;
    private Invulnerability invulnerability;
    private IKillable owner;

    public event Action<int> OnDamaged;
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        owner = GetComponent<IKillable>();
        maxHealth = SetDefaultHealth(maxHealth);
        invulnerability = GetComponent<Invulnerability>();
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
        if (invulnerability != null && invulnerability.IsInvulnerable)
            return;
        
        currentHealth -= damageAmount;
        
        OnDamaged?.Invoke(damageAmount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        if (invulnerability != null)
        {
            invulnerability.Activate();
        }
        
        if (currentHealth <= minHealth)
        {
            owner.Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
