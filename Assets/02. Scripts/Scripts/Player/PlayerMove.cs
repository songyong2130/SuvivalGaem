//TODO: 달리기 시 화면 FOV가 커질때 감도 잡기, 코드 리펙토링 하기
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어 데이터 참조")]
    [SerializeField] private PlayableCharStat playerStat;
    [SerializeField] private CharacterController cc_;
    [SerializeField] private CinemachineCamera virtualCam;

    [Header("인게임 설정")]
    [SerializeField] private float jumpPower = 5f;
    
    [Header("카메라 연출")]
    [SerializeField] private float walkFOV = 60f;
    [SerializeField] private float runFOV = 75f;
    [SerializeField] private float fovChangeSpeed = 5f;
    
    private float gravity = -50f;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSprintPressed = false;
    private bool isSprinting = false;


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
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    /// <summary>
    /// 플레이어의 점프를 감지하는 함수(조건 있음)
    /// </summary>
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && isGrounded)
        {
            // Y축 최대지점까지의 속도값을 구하기 위해 jumpPower까지 도달하는 Yt
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }
    /// <summary>
    /// 플레이어의 달리기를 감지하는 함수
    /// </summary>
    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) isSprintPressed = true;
        else if (ctx.canceled) isSprintPressed = false;
    }
    #endregion
    void Update()
    {
        isGrounded = cc_.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 만약 경사로가 있을때 플레이어가 버벅거림을 방지하기 위해 지정함
        }
        // sqrMagnitude = 벡터의 길이를 제곱한 값
        // 키 입력 감지를 더 빠르게 하기 위해 sqrMagnitude 사용
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        isSprinting = isSprintPressed && isMoving;

        HandleMovement();

        // 중력 및 y축 점프 처리
        velocity.y += gravity * Time.deltaTime;
        cc_.Move(velocity * Time.deltaTime);

        UpdateCameraFOV();

        // 밑에꺼는 이전 코드
        // Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        // move.Normalize(); // 대각선 움직임이 더 빠른 현상 고침
        // cc_.Move(move * moveSpeed* Time.deltaTime);
    }
    private void HandleMovement()
    {
        // playerStat연결 안되었을 시 기본값 10 적용
        float currentSpeed = 10f;
        if (playerStat != null) 
        {
            Debug.Log("playerStat 연결됨");
            currentSpeed = isSprinting ? playerStat.runSpeed : playerStat.walkSpeed;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.Normalize(); // 대각선 움직임이 더 빠른 현상 고침

        cc_.Move(move * currentSpeed* Time.deltaTime);
    }
    private void UpdateCameraFOV()
    {
        if (virtualCam == null) return;

        float targetFOV = isSprinting ? runFOV : walkFOV;

        virtualCam.Lens.FieldOfView = Mathf.Lerp(
            virtualCam.Lens.FieldOfView,
            targetFOV,
            Time.deltaTime * fovChangeSpeed
        );
    }
}
