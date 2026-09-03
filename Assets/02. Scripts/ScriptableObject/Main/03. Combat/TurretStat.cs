using UnityEngine;

[CreateAssetMenu(fileName = "TurretStat", menuName = "Scriptable Objects/TurretStat")]
public class TurretStat : ScriptableObject
{
    public float turretRange = 20f;
    public float damage = 20f;
    public float attackSpeed = 0.5f;

    public HitData CreateHitData(Vector3 firePos) 
    {
        return new HitData
        {
            damage = damage,
            knockbackForce = 0f,
            stunDuration = 0f,
            attackerPosition = firePos
        };
    }
    
}
