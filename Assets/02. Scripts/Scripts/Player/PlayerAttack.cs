using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float meleeDmg = 10f;

    // SerializeField로 이미 인스펙터에서 수정 가능한데 사용하는 이유 :
    // 지정한 meleeDmg값을 readonly로 하여 읽기만 가능하고 수정 불가하게 하기 위해
    public float MeleeDmg => meleeDmg;
    private bool isAttack;
    [Header("히트박스")]
    [SerializeField] private Vector3 meleeBoxSize = new Vector3(6f,6f,6f); // <- 아마 기본공격 범위로 할듯 ? (수정가능성 있음)
    [SerializeField] private float attackOffset = 2f; // 플레이어 정면 
    [SerializeField] private float attackHeightOffset = 1f; // 플레이어 위아래

    private Vector3 meleeBoxPosition;
    private PlayerMove playerMove;

    void Awake()
    {
        if(player != null)
        {
            if (player == null) player = gameObject;

            // GetComponent를 사용하는 이유
            // 오브젝트의 인스펙터에 접근 -> 
            // 지정한 스크립트 불러옴 -> 
            // 그 안에 메소드나 변수 수정 가능하게 함
            playerMove = player.GetComponent<PlayerMove>();
        }
    }
    /// <summary>
    /// 플레이어의 공격입력을 감지하는 함수
    /// </summary>
    /// <param name="Attack"></param>
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if(playerMove == null) return;

        if(ctx.started && !isAttack)
        {
            isAttack = true;
            // 캐릭터의 시작점 (0,0,0) + 캐릭터 몸통 기준 앞쪽 * 1.5m + 
            meleeBoxPosition = transform.position + transform.forward * 1.5f;
            Collider[] attackArray = Physics.OverlapBox(
                    meleeBoxPosition,
                    meleeBoxSize * 0.5f,
                    transform.rotation
                );

            foreach(Collider collider in attackArray)
            {
                if (collider.TryGetComponent(out Enemy enemy))
                {
                    enemy.takeDamage(meleeDmg);
                    Debug.Log("적 히트!" + collider.name);
                }
            }
            // 코루틴 이용 -> 개인적으로 코루틴이 쉽고 간단하게 구현 가능했음
            StartCoroutine(AutoReleaseAttack(0.2f));
        }
    }
    private IEnumerator AutoReleaseAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndAttack();
    }
    public void EndAttack()
    {
        isAttack = false;
    }
    /// <summary>
    /// 카메라(플레이어 시점)가 바라보는 방향, 각도 계산 3d 히트박스 위치 잡음
    /// </summary>
    private Vector3 GetBoxCenter()
    {
        // 만약 playerCamera가 연결되지 않았다면 움직이지 않는 기본 3D히트박스로 설정됨
        if(playerCamera == null)
        {
            Debug.LogWarning("[PlayerAttack] playerCamera가 연결되지 않음");   
            Transform charPos = (player != null) ? player.transform : transform;

            return charPos.position 
                + (charPos.forward * attackOffset)
                + (Vector3.up * attackHeightOffset);
        }
        // 1 : 기준을 플레이어 눈높이로 맞춤
        Vector3 cameraPos = (player != null) ? player.transform.position + Vector3.up * attackHeightOffset : transform.position;
        // 2 : 카메라 정면 방향으로 attackOffset만큼 앞으로 땡김
        // - 카메라 위를 보고 있으면 playerCamera.forward 방향도 위로 향해 히트박스가 위로감
        Vector3 finalPos = cameraPos + (playerCamera.forward * attackOffset);
        return finalPos;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 boxCenter = GetBoxCenter();
        Quaternion boxRotation = (player != null) ? player.transform.rotation : transform.rotation;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero,meleeBoxSize);
    }
}
