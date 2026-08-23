using UnityEngine;

// 이제부터 적용되는 애니메이션은 여기에 저장될 예정
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        // GetComponentInChildren : 자식 요소에 있는 Animator를 불러옴
        animator = GetComponentInChildren<Animator>();
    }
    /// <summary>
    /// M1 공격 (나중에 콤보 연결 예정) 애니메이션
    /// </summary>
    public void AttackAnim()
    {
        if (animator != null) animator.SetTrigger("Attack");
    }
}
