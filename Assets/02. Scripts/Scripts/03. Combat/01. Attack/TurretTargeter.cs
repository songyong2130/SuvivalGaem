using Unity.Mathematics;
using UnityEngine;

public class TurretTargeter : MonoBehaviour
{
    [SerializeField] private TurretStat stat;
    [SerializeField] private float detectCycle = 0.15f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform turretHead;
    public Enemy currentTarget {get; private set;}
    private float searchTimer;
    
    public void SearchTarget()
    {
        searchTimer += Time.deltaTime;
        if (searchTimer >= detectCycle)
        {
            IsTargeting();
            searchTimer -= detectCycle;
        }
    }
    public Enemy IsTargeting()
    {
        float shortestDistTarget = math.INFINITY;
        currentTarget = null;
        Collider[] EnemyInRange = Physics.OverlapSphere(turretHead.position, stat.turretRange, targetLayer);
        foreach (var rangeEnemy in EnemyInRange)
        {
            if (rangeEnemy.TryGetComponent<Enemy>(out Enemy enemy)) // out : 계산 결과를 메서드 외부로 전달해준다
            {
                float distEnemy = Vector3.Distance(turretHead.position, enemy.transform.position);
                if (shortestDistTarget > distEnemy)
                {
                    shortestDistTarget = distEnemy;
                    currentTarget = enemy;
                }
            }
        }
        return currentTarget;
    }
}
