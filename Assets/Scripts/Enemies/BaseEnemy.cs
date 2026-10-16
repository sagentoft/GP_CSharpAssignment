using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IKillable
{
    public event Action OnDeath;

    [SerializeField] private float separationRadius = 1.5f;
    [SerializeField] private float separationStrength;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField, Min(1)] private int experienceReward = 1;
    [SerializeField] protected float moveSpeed = 3f;

    [SerializeField] private AudioClip hitSound;
    
    protected Transform target;
    
    protected Health health;
    private int scoreValue = 1;
    private readonly Collider[] nearbyEnemies = new Collider[16];
    
    public int ExperienceReward => experienceReward;


    protected virtual void Awake()
    {
        health = GetComponent<Health>();
    }

    protected virtual void OnEnable()
    {
        health.OnDamaged += PlayHitSound;
    }

    protected virtual void OnDisable()
    {
        health.OnDamaged -= PlayHitSound;
    }

    public void SetTarget(Transform newTarget)
    {
        target =  newTarget;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (target == null)
            return;
        
        Vector3 directionToPlayer = target.position - transform.position;
        directionToPlayer.y = 0f;
        
        Vector3 separation = CalculateSeparation();
        
        Vector3 movementDirection = (directionToPlayer.normalized + separation * separationStrength).normalized;
        
        transform.position += movementDirection * (moveSpeed * Time.deltaTime);
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separation = Vector3.zero;

        int count = Physics.OverlapSphereNonAlloc(transform.position, separationRadius, nearbyEnemies, enemyLayer);

        for (int i = 0; i < count; i++)
        {
            Collider other = nearbyEnemies[i];

            if (other.transform == transform)
                continue;
            
            Vector3 away = transform.position - other.transform.position;
            away.y = 0f;
            
            float distance = away.magnitude;

            if (distance > 0.001f)
            {
                separation += away.normalized / distance;
            }
        }
        
        return separation;
    }

    public void Die()
    {
        GameManager.Instance.AddScore(scoreValue);
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
    
    private void PlayHitSound(int obj)
    {
        AudioManager.Instance.PlaySFX(hitSound);
    }
    
}
