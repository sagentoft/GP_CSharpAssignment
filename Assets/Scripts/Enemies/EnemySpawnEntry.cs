using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemySpawnEntry
{
    public BaseEnemy prefab;
    public float unlockTime;
    
    [FormerlySerializedAs("weight")] public int startingWeight = 10;
    public int finalWeight = 10;
    public float transitionDuration = 120f;
}
