using UnityEngine;
using System.Linq;

public class SkillCaster : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private PlayableCharStat playerStat;

    // void Awake()
    // {
    //     playerStat = GetComponent<PlayableCharStat>();
    // }
    public void CastSkill(PlayerSkill data, Transform firePoint)
    {
        if (data == null) return;

        Transform origin = (firePoint != null) ? firePoint : transform;
        float TotalDmg = (playerStat != null) ? playerStat.baseDmg : 10f;

        switch (data.ExecutionType)
        {
            case SkillExecutionType.InstantArea :
                CastAreaSkill(data, origin, TotalDmg);
                break;
            case SkillExecutionType.Projectile :
                CastProjectileSkill(data, origin, TotalDmg);
                break;
        }
    }

    private void CastAreaSkill(PlayerSkill data, Transform origin, float totalDmg)
    {
        Collider[] hits = null;

        Vector3 attackCenter = origin.position + (origin.forward * data.attackOffset);
        switch (data.areaType)
        {
            case SkillAreaType.Sphere:
                hits = Physics.OverlapSphere(attackCenter, data.radius, enemyLayer);
                break;
            case SkillAreaType.Box:
                hits = Physics.OverlapBox(attackCenter, data.boxSize * 0.5f, origin.rotation, enemyLayer);
                break;
            case SkillAreaType.Fan:
                // LINQ(Language Integrated Query) : 데이터를 조회, 필터링, 변환함
                hits = Physics.OverlapSphere(attackCenter, data.radius, enemyLayer)
                        .Where(col => Vector3.Angle(origin.forward, (col.transform.position - origin.position).normalized) <= data.angle * 0.5f)
                        .ToArray();
                break;
        }
        if (hits == null) return;
        HitData hitData = data.CreateHitData(totalDmg, transform.position);
        foreach(var hit in hits)
        {
            if (hit.TryGetComponent(out Enemy enemy)) 
                enemy.TakeDamage(hitData);
        }
    }
    private void CastProjectileSkill(PlayerSkill data, Transform origin, float totalDmg)
    {
        if (data.projectilePrefab == null) return;

        // Instantiate(a,b,c) : 복제본 생성
        // a : 새로운 탄환 프리펩
        // b : 현재 오브젝트 위치에서 생성
        // c : 현재 오브젝트 회전에서 생성
        GameObject projObj = Instantiate(data.projectilePrefab, origin.position, origin.rotation);

        if (projObj.TryGetComponent(out SkillProjectile projectile))
        {
            projectile.setUp(data, totalDmg, transform.position);
        }
    }
}
