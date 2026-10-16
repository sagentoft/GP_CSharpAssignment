using System;
using UnityEngine;

public class BerserkerEnemy : BaseEnemy
{
    [SerializeField] private float enragedSpeed = 5f;
    [SerializeField, Range(0f, 1f)]
    private float enrageThreshold = 0.4f;
    
    private float normalSpeed;

    protected override void Awake()
    {
        base.Awake();
        
        normalSpeed = moveSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        moveSpeed = normalSpeed;
        health.OnHealthChanged += HandleHealthChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        health.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        
        moveSpeed = healthPercentage <= enrageThreshold ? enragedSpeed : normalSpeed;
    }
}
