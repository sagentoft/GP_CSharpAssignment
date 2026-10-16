using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumberSpawner : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private Vector3 spawnOffset = Vector3.up;
    [SerializeField] private float randomOffset = 0.25f;
    [SerializeField] private Color damageColor = Color.white;

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
    }

    private void HandleDamage(int damage)
    {
        Vector3 randomPositionOffset = new Vector3(Random.Range(-randomOffset, randomOffset),
            0,
            Random.Range(-randomOffset, randomOffset)
            );
        
        Vector3 spawnPosition = transform.position + spawnOffset + randomPositionOffset;
        
        DamageNumber damageNumber = PooledObjects.Instance.Get<DamageNumber>(damageNumberPrefab,
            spawnPosition);
        
        damageNumber.Show(damage, damageColor);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
