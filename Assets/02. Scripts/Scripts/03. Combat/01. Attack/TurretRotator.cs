using UnityEngine;
public class TurretRotator : MonoBehaviour
{
    [SerializeField] private TurretTargeter targeter;
    [SerializeField] private Transform xRotation;
    [SerializeField] private Transform yRotation;
    [SerializeField] private float spinSpeed = 10f;
    [SerializeField] private float allowAngle = 5f;
    public bool IsAimed {get; private set;}
    public void Rotate(Enemy target)
    {
        if (target == null) return;
        TurretRotate(target);
    }
    public void TurretRotate(Enemy target)
    {
        if (target == null) return;
        Vector3 yDir = (target.transform.position - yRotation.position).normalized;
        yDir.y = 0f; yDir = yDir.normalized; // Angle은 정규화 한 값이 더 정확함

        float angle = Vector3.Angle(transform.forward, yDir);
        if (angle <= allowAngle) IsAimed = true; else IsAimed = false;

        if (yDir.sqrMagnitude >=  0.001f)
        {
            Quaternion targetRotation= Quaternion.LookRotation(yDir);
            yRotation.rotation = Quaternion.Slerp(yRotation.rotation, targetRotation, spinSpeed * Time.deltaTime);
        }
        Vector3 xDir = (target.transform.position - xRotation.position).normalized;
        if (xDir.sqrMagnitude >= 0.001f)
        {
            Vector3 localTargetPos = yRotation.InverseTransformPoint(target.transform.position);

            float targetAngleX = Mathf.Atan2(localTargetPos.y, localTargetPos.z) * Mathf.Rad2Deg;
            Quaternion targetRotX = Quaternion.Euler(targetAngleX, 0, 0);

            xRotation.localRotation = Quaternion.Slerp(xRotation.localRotation, targetRotX, spinSpeed * Time.deltaTime);
        }
    }
}
