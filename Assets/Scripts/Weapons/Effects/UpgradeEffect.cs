using System;
using UnityEngine;

[Serializable]
public abstract class UpgradeEffect
{
    public abstract void Apply(GameObject target, UpgradeData upgrade);
}
