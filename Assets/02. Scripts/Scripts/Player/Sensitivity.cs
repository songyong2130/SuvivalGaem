using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : MonoBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private Transform playerCamera;
    
    [Header("인게임 설정")]
    [SerializeField] private float mouseSensitivity = 15f; 
    private float xRotation = 0f;
    private Vector2 lookInput;
    
    /// <summary>
    /// 플레이어의 마우스 움직임을 감지하는 함수
    /// </summary>
    /// <param name="Look"></param>
    public void OnLook(InputAction.CallbackContext ctx)
    {
        
        if (ctx.canceled)
        {
            lookInput = Vector2.zero ;
            return;
        }
        if(ctx.performed) lookInput = ctx.ReadValue<Vector2>();
    }
    // 감도는 Update() 함수에서
    void Update()
    {
        // 상하,좌우 마우스 각도 = 마우스 x,y좌표 움직임 * 마우스 감도 * 델타 타임
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 60f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
