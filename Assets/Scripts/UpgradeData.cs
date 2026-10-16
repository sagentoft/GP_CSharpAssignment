using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private UpgradeLevel[] levels;

    public WeaponData WeaponData => weaponData;
    
    public string UpgradeName => upgradeName;
    public string Description => description;
    public Sprite Icon => icon;
    
    public int MaxLevel => levels.Length;

    public UpgradeLevel GetLevel(int level)
    {
        return levels[level - 1];
    }
}
