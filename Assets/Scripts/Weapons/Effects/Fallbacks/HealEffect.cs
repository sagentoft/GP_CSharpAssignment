using UnityEngine;

[System.Serializable]
public class HealEffect : UpgradeEffect
{
    [SerializeField, Range(0f, 1f)] 
    private float healPercentage = 0.25f;

    public override void Apply(GameObject target, UpgradeData upgrade)
    {
        Health health = target.GetComponent<Health>();

        if (health == null)
            return;
        
        int amount = Mathf.CeilToInt(health.MaxHealth * healPercentage);
        health.Heal(amount);

    }
}
