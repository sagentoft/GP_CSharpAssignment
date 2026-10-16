using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    
    private float moveSpeed;
    private int damage;

    private float remainingLifetime;
    private Vector3 direction;
    private int remainingPierce;

    private readonly HashSet<IDamageable<int>> hitTargets = new();

    private void OnEnable()
    {
        remainingLifetime = lifeTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        remainingLifetime -= Time.deltaTime;
        if (remainingLifetime <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }
        
        transform.position += direction * (moveSpeed * Time.deltaTime);
    }
    
    public void Initialize(Vector3 newDirection, float newMoveSpeed, int newDamage, int pierce)
    {
        direction = newDirection.normalized;
        moveSpeed = newMoveSpeed;
        damage = newDamage;

        remainingPierce = Mathf.Max(0, pierce);
        hitTargets.Clear();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable<int> damageable = other.GetComponent<IDamageable<int>>();

        if (damageable == null)
            return;

        if (!hitTargets.Add(damageable))
            return;

        damageable.Damage(damage);

        if (remainingPierce > 0)
        {
            remainingPierce--;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
