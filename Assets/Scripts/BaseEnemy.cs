using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IKillable
{
    public event Action OnDeath;
    
    [SerializeField] private float moveSpeed = 3f;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target =  newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

    public void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
    
}
