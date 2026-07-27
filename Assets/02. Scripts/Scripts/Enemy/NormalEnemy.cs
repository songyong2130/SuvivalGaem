using UnityEngine;

public class NormalEnemy : Enemy // Enemy클래스 상속
{
    [Header("적 설정")]
    [SerializeField] private EnemySpec enemySpec;

    protected override void Awake()
    {
        if (enemySpec != null)
        {
            maxHp = enemySpec.maxHp;
        }
        base.Awake(); // 부모 클래스의 Awake() 호출
    }
}
