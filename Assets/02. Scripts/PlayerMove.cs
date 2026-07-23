using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("인게임 설정")]
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpPower = 5f;
    
    [Header("플레이어 설정")]
    [SerializeField] private CharacterController cc_;
    
    private float gravity = -16.8f;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if(cc_ == null) 
            cc_ = GetComponent<CharacterController>();
    }
    #region newInputSystem
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }
    #endregion
    void Update()
    {
        isGrounded = cc_.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.Normalize();
        cc_.Move(move * moveSpeed* Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc_.Move(velocity * Time.deltaTime);
    }
}
