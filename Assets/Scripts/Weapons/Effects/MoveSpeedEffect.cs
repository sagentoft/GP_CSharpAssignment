using System;
using UnityEngine;

[Serializable]
public class MoveSpeedEffect : UpgradeEffect
{
    [SerializeField] private float multiplier = 1f;
    
    public override void Apply(GameObject target, UpgradeData upgrade)
    {
        PlayerController controller = target.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ModifyMoveSpeed(multiplier);
        }
    }
}
