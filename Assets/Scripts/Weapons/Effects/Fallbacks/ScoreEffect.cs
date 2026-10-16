using UnityEngine;

[System.Serializable]
public class ScoreEffect : UpgradeEffect
{
    [SerializeField] private int scoreToAdd;
    
    public override void Apply(GameObject target, UpgradeData upgrade)
    {
        GameManager.Instance.AddScore(5);
    }
}
