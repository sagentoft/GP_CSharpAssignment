using UnityEngine;

[CreateAssetMenu(fileName = "New Fallback Reward", menuName = "Upgrades/Fallback Reward")]
public class FallbackReward : ScriptableObject
{
    [SerializeField] private string rewardName;

    [SerializeReference]
    private UpgradeEffect effect;
    
    public string RewardName => rewardName;

    public void Apply(GameObject target)
    {
        effect?.Apply(target, null);
    }
}
