using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private GameObject player;
    [SerializeField] private float meleeDmg = 10f;
    
    public float MeleeDmg => meleeDmg;
    private bool isAttack;
    [Header("히트박스")]
    [SerializeField] private Vector3 meleeBoxSize = new Vector3(4f,2f,2f);
    [SerializeField] private float attackOffset = 2f;
    [SerializeField] private float attackHeightOffset = 1.5f;

    private Vector3 meleeBoxPosition;
    private PlayerMove playerMove;
    private Rigidbody rb;

    void Awake()
    {
        if(player != null)
        {
            if (player == null) player = gameObject;

            playerMove = player.GetComponent<PlayerMove>();
            rb = player.GetComponent<Rigidbody>();
        }
    }
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if(playerMove == null && rb == null) return;

        if(ctx.started && !isAttack)
        {
            isAttack = true;

            meleeBoxPosition = transform.position + transform.forward * 1.5f + Vector3.up;
            Collider[] attackArray = Physics.OverlapBox(
                            meleeBoxPosition,
                            meleeBoxSize * 0.5f,
                            transform.rotation
                        );

            foreach(Collider collider in attackArray)
            {
                if (collider.CompareTag("Enemy"))
                {
                    
                }
            }
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + transform.forward * attackOffset + Vector3.up;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero,meleeBoxSize);
    }
}
