using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;

    private float remainingLifetime;
    private Vector3 direction;
    //private Collider col;

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
    
    public void Initialize(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable<int> damageable = other.GetComponent<IDamageable<int>>();

        if (damageable != null)
        {
            damageable.Damage(damage);
            gameObject.SetActive(false);
            
        }

    }
}
