using UnityEngine;

[System.Serializable]
public class PickupRadiusEffect : UpgradeEffect
{
    [SerializeField] private float multiplier = 1f;
    
    public override void Apply(GameObject target, UpgradeData upgrade)
    {
        PickupMagnet magnet = target.GetComponent<PickupMagnet>();

        if (magnet != null)
        {
            magnet.ModifyPickupRadius(multiplier);
        }
    }
}
