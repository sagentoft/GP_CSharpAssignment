using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChargerEnemy : BaseEnemy
{
    private enum ChargerState
    {
        Approach,
        Windup,
        Charge
    }

    [SerializeField] private Transform chargeIndicator;
    [SerializeField] private float chargeRange = 6f;
    [SerializeField] private float windupDuration = 0.75f;
    [SerializeField] private float chargeSpeed = 12f;
    [SerializeField] private float overshootDistance = 3f;
    [SerializeField] private float chargeCooldown = 5f;

    private ChargerState currentState;
    private Vector3 chargeDestination;
    private float stateTimer;
    private float cooldownTimer;


    protected override void Awake()
    {
        base.Awake();
        
        chargeIndicator.SetParent(null, true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        currentState = ChargerState.Approach;
        stateTimer = 0f;
        cooldownTimer = chargeCooldown;
        chargeIndicator.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        if (chargeIndicator != null)
            chargeIndicator.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (chargeIndicator != null)
            chargeIndicator.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (target == null)
            return;
        
        switch (currentState)
        {
            case ChargerState.Approach:
                Approach();
                break;
            case ChargerState.Windup:
                Windup();
                break;
            case ChargerState.Charge:
                Charge();
                break;
        }
    }

    private void Approach()
    {
        cooldownTimer -= Time.deltaTime;
        
        base.Update();
        
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= chargeRange && cooldownTimer <= 0f)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            
            chargeDestination = target.position + direction * overshootDistance;
            chargeIndicator.gameObject.SetActive(true);
            UpdateChargeIndicator();
            
            currentState = ChargerState.Windup;
            stateTimer = windupDuration;
        }
    }

    private void Windup()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            chargeIndicator.gameObject.SetActive(false);
            currentState = ChargerState.Charge;
        }
    }

    private void Charge()
    {
        transform.position = Vector3.MoveTowards(transform.position, chargeDestination,
            chargeSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, chargeDestination) <= 0.01f)
        {
            currentState = ChargerState.Approach;
            cooldownTimer = chargeCooldown;
        }
    }

    private void UpdateChargeIndicator()
    {
        Vector3 start = transform.position;
        Vector3 end = chargeDestination;

        Vector3 direction = end - start;
        direction.y = 0f;
        
        float length = direction.magnitude;

        chargeIndicator.position = (start + end) * 0.5f + Vector3.up * 0.05f;
        chargeIndicator.rotation = Quaternion.LookRotation(direction);
        chargeIndicator.localScale = new Vector3(0.8f, 0.02f, length);
    }
}
