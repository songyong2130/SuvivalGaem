using Unity.Cinemachine;
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
    
    public void OnLook(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed && !ctx.started) return;
        lookInput = ctx.ReadValue<Vector2>();
    }
    
    void Update()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 60f);

        if (playerCamera != null)
        {
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        lookInput = Vector2.zero;
    }
}
