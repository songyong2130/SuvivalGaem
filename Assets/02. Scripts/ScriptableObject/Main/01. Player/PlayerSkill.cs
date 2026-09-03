using UnityEngine;

public enum SkillExecutionType {InstantArea, Projectile}
public enum SkillAreaType {Sphere, Box, Fan}

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "Scriptable Objects/PlayerSkill")]
public class PlayerSkill : ScriptableObject
{
    [Header("기본 정보")]
    public string s_Name;
    public float staminaCost = 20f;
    public float cooldown = 10f;

    [Header("데미지 배율 / 고정 데미지")]
    public float damageMul = 1.5f;
    public float flatBonusDmg = 0f;
    
    
    [Header("넉백 및 CC")]
    public float knockbackForce = 0f;
    public float stunDuration = 0f;

    [Header("스킬 선택 방식")]
    public SkillExecutionType ExecutionType;

    [Header("근접 타입 선택 및 범위 지정")]
    public SkillAreaType areaType;
    public float radius = 5f;
    public float angle = 90f;
    public Vector3 boxSize = new Vector3(3f,2f,10f);
    public float attackOffset = 2f;

    [Header("원거리 투사체 설정")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float projectileLifeTime = 5f;

    public HitData CreateHitData(float playerTotalDmg, Vector3 attackerPos)
    {
        float finalDmg = (playerTotalDmg * damageMul) + flatBonusDmg;
        return new HitData
        {
            damage = finalDmg,
            knockbackForce = this.knockbackForce,
            stunDuration = this.stunDuration,
            attackerPosition = attackerPos
        };
    }
}
