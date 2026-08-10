using UnityEngine.UI;
using UnityEngine;

public class HP_BarUI : MonoBehaviour
{
    [SerializeField] private Image healthBar; // <- filled로 지정
    [SerializeField] private PlayerStatus playerStatus; // PlayerStatus 스크립트 연결
    
    private void OnEnable()
    {
        if (playerStatus != null)
        {
            playerStatus.onHealthChanged.AddListener(UpdateHealthBar);
        }
    }
    private void OnDisable()
    {
        if (playerStatus != null) 
        {
            playerStatus.onHealthChanged.RemoveListener(UpdateHealthBar);
        }
    }
    /// <summary>
    /// PlayerStatus의 onHealthChanged에 넣어줄 함수 
    /// </summary>
    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        if (healthBar != null)
        {
            //0.0 ~ 1.0 사이 값 계산
            healthBar.fillAmount = currentHp / maxHp;
        }
    }
}
