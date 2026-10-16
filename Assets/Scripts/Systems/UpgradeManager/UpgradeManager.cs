using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private List<UpgradeInstance> upgrades = new();

    public void AcquireUpgrade(UpgradeData data)
    {
        UpgradeInstance upgrade = GetUpgrade(data);

        if (upgrade == null)
        {
            upgrade = new UpgradeInstance(data);
            upgrades.Add(upgrade);
        }
        
        upgrade.Upgrade(gameObject);
    }

    public UpgradeInstance GetUpgrade(UpgradeData data)
    {
        return upgrades.Find(upgrade => upgrade.Data == data);
    }

    public bool CanAcquireUpgrade(UpgradeData data)
    {
        UpgradeInstance upgrade = GetUpgrade(data);
        
        return upgrade == null || !upgrade.IsMaxLevel;
    }

    public int GetNextUpgradeLevel(UpgradeData data)
    {
        UpgradeInstance upgrade = GetUpgrade(data);
        return upgrade == null ? 1 : upgrade.CurrentLevel + 1;
    }
}
