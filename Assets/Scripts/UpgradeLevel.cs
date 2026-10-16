using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeLevel
{
    [SerializeField] private string description;

    [SerializeReference]
    private List<UpgradeEffect> effects = new();
    
    public string Description => description;

    public void Apply(GameObject target, UpgradeData upgrade)
    {
        foreach (UpgradeEffect effect in effects)
        {
            effect.Apply(target, upgrade);
        }
    }
}
