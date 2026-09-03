using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMover : MonoBehaviour
{
    private NavMeshAgent nav;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

        // SamplePosition : 어떤 위치에서 가장 가까운 NavMesh위의 위치를 찾아주는 함수
        // SamplePosition(찾아볼 기준 위치, 찾은 Nav정보를 담을 함수, 기준 위치에서 최대 n거리 안에서 NavMesh 찾음, 모든 NavMesh Area를 대상으로 잡음)
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
        {
            nav.Warp(hit.position);
        }
    }
    
    public void Init(EnemySpec spec)
    {
        if (nav == null) nav = GetComponent<NavMeshAgent>();
        nav.isStopped = false;
        nav.speed = spec.moveSpeed;
    }

    // 플레이어 추적 함수
    public void Moving(Vector3 targetPosition)
    {
        if (nav != null && nav.enabled && nav.isOnNavMesh) 
        {
            nav.isStopped = false;
            nav.SetDestination(targetPosition); // 목적지를 타겟(플레이어) 위치로 지정
        }
    }
    // 적 멈추게 하는 코드
    public void Stop()
    {
        // 넉백 및 스턴을 Enemy에서 통합관리를 하는데 아마 EnemyAI에서는 스턴 상태일떄 불러와짐
        // 이때 스턴이 되면서 agent.enabled가 비활성화 됨 (하지만 EnemyAI에서는 계속 mover.stop을 불러와 오류 발생)
        // 이를 해결하기 위해 if문으로 한번 걸러내서 오류 안뜨게 수정
        if (nav != null && nav.enabled && nav.isOnNavMesh) nav.isStopped = true;
    }
    // 적이 플레이어 기준으로 정면 방향을 보는 함수
    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 targetDirection = (targetPosition - transform.position).normalized;
        targetDirection.y = 0; 
        
        if (targetDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation(targetDirection);
    }
}
