using System;
using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float flashDuration = 0.1f;
    
    private Color originalColor;

    private void Awake()
    {
        originalColor = targetRenderer.material.color;
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        targetRenderer.material.color = originalColor;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
    }
    
    private void HandleDamage(int damage)
    {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        targetRenderer.material.color = Color.white;
        
        yield return new WaitForSeconds(flashDuration);
        
        targetRenderer.material.color = originalColor;
    }
}
