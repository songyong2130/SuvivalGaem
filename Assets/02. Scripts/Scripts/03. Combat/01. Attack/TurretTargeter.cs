using UnityEngine;

public class TurretTargeter : MonoBehaviour
{
    [SerializeField] private float turretRange = 30f;
    [SerializeField] private float detectCycle = 0.15f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform turretHead;
    public Enemy currentTarget {get; private set;}
    private float searchTimer;
    private void Update()
    {
        searchTimer += Time.deltaTime;
        if (searchTimer >= detectCycle)
        {
            IsTargeting();
            searchTimer = 0f;
        }
    }
    private Enemy IsTargeting()
    {
        float shortestDistTarget = float.MaxValue;
        currentTarget = null;
        Collider[] EnemyInRange = Physics.OverlapSphere(turretHead.position, turretRange, targetLayer);
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
