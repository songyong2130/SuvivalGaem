using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : MonoBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CinemachineCamera virtualCam;
    
    [Header("인게임 설정")]
    [Range (0.1f,10.0f)]
    // [Range (float, float)] : 유니티 인스펙터 창에서 슬라이더로 지정 가능
    [SerializeField] private float mouseSensitivity = 1f; 
    [SerializeField] private float baseFOV = 60f;

    private float Base_Yaw = 0.022f;

    private float xRotation = 0f;
    private Vector2 lookInput;
    
    /// <summary>
    /// 플레이어의 마우스 움직임을 감지하는 함수
    /// </summary>
    public void OnLook(InputAction.CallbackContext ctx)
    {
        
        if (ctx.canceled)
        {
            lookInput = Vector2.zero ;
            return;
        }
        if(ctx.performed) lookInput = ctx.ReadValue<Vector2>();
    }
    // 최종 감도계산은 Update() 함수에서
    void Update()
    {
        float fovMultiplier = 1f;

        if (virtualCam != null)
        {
            float currentFOV = virtualCam.Lens.FieldOfView;

            float currentRad = currentFOV * 0.5f * Mathf.Deg2Rad;
            float baseRad = baseFOV * 0.5f * Mathf.Deg2Rad;
            
            fovMultiplier = Mathf.Tan(currentRad) / Mathf.Tan(baseRad);
        }

        float adjustedSensitivity = Base_Yaw * mouseSensitivity * fovMultiplier;

        // 상하,좌우 마우스 각도 = 마우스 x,y좌표 움직임 * 마우스 감도
        float mouseX = lookInput.x * adjustedSensitivity * 8f;
        float mouseY = lookInput.y * adjustedSensitivity * 8f;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 60f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
