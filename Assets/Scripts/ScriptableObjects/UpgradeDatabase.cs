using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "Upgrades/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<UpgradeData> upgrades;
    
    public IReadOnlyList<UpgradeData> Upgrades => upgrades;
}
