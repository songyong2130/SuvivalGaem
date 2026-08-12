using UnityEngine;

public class AnimCheck : MonoBehaviour
{
    private PlayerAttack playerAttack;

    void Start()
    {
        // GetComponentInParent : 부모 요소에 있는 PlayerAttack를 불러옴
        playerAttack = GetComponentInParent<PlayerAttack>();
    }
    // 애니메이션 이벤트를 통해 공격이 적에게 적중했을때 데미지를 줄 예정
    public void OnAnimCheck()
    {
        if (playerAttack != null)
        {
            playerAttack.ExecuteAnimRelay();
        }
    }
}
