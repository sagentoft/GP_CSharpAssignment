using UnityEngine;

public class UpgradeInstance
{
    private UpgradeData data;
    private int currentLevel;
    
    public UpgradeData Data => data;
    public bool IsMaxLevel => currentLevel >= data.MaxLevel;
    public int CurrentLevel => currentLevel;

    public UpgradeInstance(UpgradeData data)
    {
        this.data = data;
        currentLevel = 0;
    }

    public void Upgrade(GameObject target)
    {
        if (currentLevel >= data.MaxLevel)
        {
            return;
        }
        
        currentLevel++;
        
        UpgradeLevel level = data.GetLevel(currentLevel);
        level.Apply(target, data);
    }
}
