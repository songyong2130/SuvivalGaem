using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("스크립트 파일 설정")]
    [SerializeField] private CharacterController cc;

    [Header("대쉬 설정")]
    [SerializeField] private float dashSpeed = 40f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooltime = 2f;
    [SerializeField] private float dashStmCost = 20f;
    private bool isDashing = false;
    public bool IsDashing => isDashing;
    private bool isCooldown = false;
    private PlayerStamina stamina;
    private PlayerMove playerMove;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        stamina = GetComponent<PlayerStamina>();
        playerMove = GetComponent<PlayerMove>();
    }
    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !isDashing && !isCooldown)
        {
            if (stamina != null && stamina.UseStamina(dashStmCost))
            {
                StartCoroutine(DashRoutine());
            } 
        }
    }
    private IEnumerator DashRoutine()
    {
        isDashing = true;

        Vector2 input = (playerMove != null) ? playerMove.MoveInput : Vector2.zero;

        Vector3 moveDir = transform.right * input.x + transform.forward * input.y;
        Vector3 dashDirection = moveDir.sqrMagnitude > 0.01f ? moveDir.normalized : transform.forward;
        float timer = 0f;
        while (timer < dashDuration)
        {
            cc.Move(dashDirection * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        isCooldown = true;
        yield return new WaitForSeconds(dashCooltime);
        isCooldown = false;
    }   
}
