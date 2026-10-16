using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Upgrades/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private Weapon weaponPrefab;
    
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite icon;

    [SerializeField] private int baseDamage;
    [SerializeField] private float baseCooldown;
    [SerializeField] private float baseRange = 2f;
    
    public string WeaponName => weaponName;
    public Sprite Icon => icon;

    public Weapon WeaponPrefab => weaponPrefab;

    public int BaseDamage => baseDamage;
    public float BaseCooldown => baseCooldown;
    public float BaseRange => baseRange;
}

