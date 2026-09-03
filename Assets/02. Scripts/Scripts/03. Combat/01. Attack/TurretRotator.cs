using UnityEngine;

public class TurretRotator : MonoBehaviour
{
    [SerializeField] private TurretTargeter targeter;
    [SerializeField] private Transform xRotation;
    [SerializeField] private Transform yRotation;
    [SerializeField] private float spinSpeed = 10f;
    
    public void TurretRotate()
    {
        Vector3 playerDir = (targeter.currentTarget.transform.position - transform.position).normalized;
        playerDir.y = 0f;

        if (playerDir.sqrMagnitude <  0.001f) return;
        Quaternion targetRotation= Quaternion.LookRotation(playerDir);
        yRotation.rotation = Quaternion.Slerp(yRotation.rotation, targetRotation, spinSpeed * Time.deltaTime);
    }
}
