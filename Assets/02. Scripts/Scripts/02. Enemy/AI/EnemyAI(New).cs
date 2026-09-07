using UnityEngine;

/// <summary>
/// 적 AI 중앙 제어
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyPerception perception;
    [SerializeField]private EnemyMover mover;
    private EnemyCombat combat;
    private Enemy enemy;
    [SerializeField] private EnemySpec enemySpec;

    private bool hasTarget = false;

    private void Awake()
    {
        perception = GetComponent<EnemyPerception>();
        mover = GetComponent<EnemyMover>();
        combat = GetComponent<EnemyCombat>();
        enemy = GetComponent<Enemy>();
        
        if (enemySpec != null)
        {
            perception.Init(enemySpec);
            mover.Init(enemySpec);
            combat.Init(enemySpec);
        } 
    }
    private void Update()
    {
        if (enemy != null && enemy.isStunned)
        {
            mover.Stop();
            return;
        }
        if (!hasTarget)
        {
            if (perception.CanSeePlayer())
            {
                hasTarget = true;
            }
            else
            {
                mover.Stop();
                return;
            }
        }

        Transform player = perception.player; // 타겟 고정
        if (player == null)
        {
            hasTarget = false;
            mover.Stop();
            return;
        }
        if(perception.IsPlayerInAtkRange())
        {
            mover.Stop();
            mover.RotateTowards(player.position);
            combat.attackInput(player);
        } 
        else
        {
            mover.Moving(player.position);
        }
    }
}
