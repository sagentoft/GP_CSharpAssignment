using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Transform fill;

    private Vector3 fullScale;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        fullScale = fill.localScale;
    }

    private void OnEnable()
    {
        health.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateHealthBar;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float healthPercent = (float)currentHealth / maxHealth;
        
        fill.localScale = new Vector3(fullScale.x * healthPercent, fullScale.y, fullScale.z);
    }
}
