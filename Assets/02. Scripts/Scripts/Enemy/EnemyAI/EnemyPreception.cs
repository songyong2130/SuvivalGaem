using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    private float detectionRange;
    private float attackRange;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private Transform eyePoint;

    public Transform player {get; private set;}

    /// <summary>
    /// 적 스탯 초기화 함수 (대부분의 Init은 이 역할임)
    /// </summary>
    public void Init(EnemySpec spec)
    {
        detectionRange = spec.detectionRange;
        attackRange = spec.attackRange;
    }
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)  player = playerObj.transform;
        if (eyePoint == null) eyePoint = transform;
    }
    // 플레이어가 인지 범위 안에 있는지 판단하는 함수
    public bool IsPlayerInRange()
    {
        if(player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }
    // 플레이어가 공격 범위 안에 있는지 판단하는 함수
    public bool IsPlayerInAtkRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }
    // 적이 플레이어를 감지하는지 확인하는지 판단하는 함수
    public bool CanSeePlayer()
    {
        if(!IsPlayerInRange()) return false;

        Vector3 dirToPlayer = (player.position - eyePoint.position).normalized;
        float distance = Vector3.Distance(eyePoint.position, player.position);

        // Raycast = Ray가 Collider에 부딪히면 true, 아니면 false 반환
        // Raycast(발사 위치, 발사 방향, 충돌 정보 저장, 최대 거리, 충돌할 레이어 지정 )
        return !Physics.Raycast(eyePoint.position, dirToPlayer, distance, wallLayerMask);
    }
}
