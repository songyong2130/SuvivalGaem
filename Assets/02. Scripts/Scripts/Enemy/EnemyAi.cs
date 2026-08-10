// TODO : 플레이어 공격 구체적이게 구상
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooltime = 1f;
    [SerializeField] int damage = 10;

    private Transform player;
    private float lastAttackTime;
    private PlayerStatus playerStatus;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;

            playerStatus = playerObj.GetComponent<PlayerStatus>();
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            
            if (playerStatus == null)
            {
                Debug.LogError("Player 오브젝트에 PlayerStatus가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Player 태그가 있는 오브젝트가 없습니다.");
        }
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 감지 범위 안이고, 아직 공격 범위 밖일 때는 추적 
        if (distance <= detectionRange && distance >= attackRange)
        {
            // 플레이어 X, Z좌표와 자신의 Y값을 넣어서 좌우로만 보게 함
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPosition);
            // 이동
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooltime)
        {
            // 여기에는 플레이어 현재체력 감소 로직 추가
            playerStatus.TakeDamage(damage);
            lastAttackTime = Time.time;

            Debug.Log($"플레이어에게 {damage}피해!");
        }
    }
    // 씬 뷰에서 감지/공격 범위 확인용 로직
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
