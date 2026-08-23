#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SkillHitBox : MonoBehaviour
{
    [Header("프리뷰 히트박스")]
    [SerializeField] private PlayerSkill skillPreview;

    private void OnDrawGizmosSelected()
    {
        if (skillPreview == null) return;

        Gizmos.color = Color.red;
        Vector3 center = transform.position + (transform.forward * skillPreview.attackOffset);

        switch(skillPreview.areaType)
        {
            case SkillAreaType.Sphere : 
            Gizmos.DrawWireSphere(center, skillPreview.radius);
            break;

            case SkillAreaType.Box : 
            Matrix4x4 oldMatrix = Gizmos.matrix;
            // matrix : 행렬 | Matrix4x4 : 4x4행렬 | TRS : Transform, Rotation, Scale
            Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, skillPreview.boxSize);
            Gizmos.matrix = oldMatrix;
            break;

            case SkillAreaType.Fan :
            #if UNITY_EDITOR
            Handles.color = new Color(1f,0f,0f,0.2f);
            Vector3 leftDir = Quaternion.Euler(0, -skillPreview.angle * 0.5f ,0) * transform.forward;
            Handles.DrawSolidArc(center, Vector3.up, leftDir, skillPreview.angle, skillPreview.radius);
            break;
            #endif
        } 
    }
}
