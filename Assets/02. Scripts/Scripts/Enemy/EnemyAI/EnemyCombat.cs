using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private float damage;
    private float attackCooltime;

    private float lastAttackTime;
    public bool canAttack() => Time.time - lastAttackTime >= attackCooltime;

    public void Init(EnemySpec spec)
    {
        damage = spec.attackPower;
        attackCooltime = spec.attackcooltime;
    }

    public void attackInput(Transform target)
    {
        if (!canAttack()) return;
        if (target.TryGetComponent(out PlayerHP playerHP))
        {
            playerHP.TakeDamage(damage);
        }
        lastAttackTime = Time.time;
    }
}