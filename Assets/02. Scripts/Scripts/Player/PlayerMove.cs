using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("인게임 설정")]
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpPower = 5f;
    
    [Header("플레이어 설정")]
    [SerializeField] private CharacterController cc_;
    
    private float gravity = -29.4f;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // CharacterController가 비어있을시 CharacterController를 불러와 에러 방지
        if(cc_ == null) 
            cc_ = GetComponent<CharacterController>();
    }
    #region newInputSystem
    /// <summary>
    /// 플레이어의 움직임(WASD)를 감지하는 함수
    /// </summary>
    /// <param name="Move"></param>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    /// <summary>
    /// 플레이어의 점프를 감지하는 함수(조건 있음)
    /// </summary>
    /// <param name="Jump"></param>
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && isGrounded)
        {
            // Y축 최대지점까지의 속도값을 구하기 위해 jumpPower까지 도달하는 Yt
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }
    #endregion
    void Update()
    {
        isGrounded = cc_.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 만약 경사로가 있을때 플레이어가 버벅거림을 방지하기 위해 지정함
        }
        
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.Normalize(); // 대각선 움직임이 더 빠른 현상 고침
        cc_.Move(move * moveSpeed* Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc_.Move(velocity * Time.deltaTime);
    }
}
