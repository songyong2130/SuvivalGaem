using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
    [Header("스탯 에셋")]
    [SerializeField] private PlayableCharStat playerStat;

    public float currentHp {get; private set;} // 수정은 내부에서 외부는 읽기만 가능(read-only)
    public float maxHp => playerStat != null ? playerStat.maxHp : 100f; // 에셋에서 가져온 maxHp

    [Header("UI")]
    public UnityEvent<float, float> onHealthChanged; // 현재 체력, 최대 체력을 UI에 전달

    void Awake()
    {
        if (playerStat != null )
        {
            currentHp = playerStat.maxHp;
        }
        else
        {
            Debug.LogWarning("PlayableCharStat이 연결되지 않았습니다.");
            currentHp = 100f;
        }
    }

    void Start()
    {
        if (onHealthChanged == null)
        {
            Debug.LogError("onHealthChanged이 연결되지 않았습니다");
            return;
        }
        //체력바 UI 초기화를 위한 이벤트
        onHealthChanged.Invoke(currentHp,maxHp);
    }
    /// <summary>
    /// 데미지 입을 시 호출하는 함수
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0 , maxHp); // 0 ~ maxHp 유지
        
        onHealthChanged.Invoke(currentHp, maxHp); // UI 갱신

        if(currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("플레이어 사망");
        // 사망 애니메이션, 리스폰 처리를 여기서 함
    }
}
