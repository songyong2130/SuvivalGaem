using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HP_BarUI : MonoBehaviour
{
    [SerializeField] private Image healthBar; // <- filled로 지정
    [SerializeField] private PlayerHP playerHP; // PlayerHP 스크립트 연결
    [SerializeField] private TextMeshProUGUI text;
    
    private void OnEnable()
    {
        if (playerHP != null)
        {
            playerHP.onHealthChanged.AddListener(UpdateHealthUI);
        }
    }
    private void OnDisable()
    {
        if (playerHP != null) 
        {
            playerHP.onHealthChanged.RemoveListener(UpdateHealthUI);
        }
    }
    /// <summary>
    /// PlayerStatus의 onHealthChanged에 넣어줄 함수 
    /// </summary>
    public void UpdateHealthUI(float currentHp, float maxHp)
    {
        if (healthBar != null)
        {
            //0.0 ~ 1.0 사이 값 계산
            healthBar.fillAmount = currentHp / maxHp;
        }
        if (text != null)
        {
            text.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
        }
    }
}
