using UnityEngine;

public class TurretShooter : MonoBehaviour
{
    [Header("기본 스탯")]
    [SerializeField] private TurretStat stat;
    [Header("터렛 설정")]
    [SerializeField] private TurretTargeter target;
    [SerializeField] private Transform firePos;
    [SerializeField] private LayerMask enemyLayer;
    
    public void Shoot()
    {
        if (target == null || target.currentTarget == null ) return;
        RaycastHit hit;

        Vector3 targetDir = (target.currentTarget.transform.position - firePos.position).normalized;

        if (Physics.Raycast(firePos.position, targetDir, out hit, stat.turretRange, enemyLayer))
        {
            if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                HitData hitData = stat.CreateHitData(firePos.position);
                enemy.TakeDamage(hitData);
            }
        }
    }
}
