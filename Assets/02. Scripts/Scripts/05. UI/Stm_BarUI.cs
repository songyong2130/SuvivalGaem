using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class Stm_BarUI : MonoBehaviour
{
    [SerializeField] private Image StaminaBar; // <- filled로 지정 필요
    [SerializeField] private PlayerStamina p_Stm;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        if (p_Stm != null)
        {
            p_Stm.onStaminaChanged.AddListener(UpdateStaminaUI);
        }
    }
    private void OnDisable()
    {
        if (p_Stm != null)
        {
            p_Stm.onStaminaChanged.RemoveListener(UpdateStaminaUI);
        }
    }
    public void UpdateStaminaUI(float currentStm, float maxStm)
    {
        if (StaminaBar != null)
        {
            StaminaBar.fillAmount = currentStm / maxStm;
        }
        if (text != null)
        {
            text.text = $"{Mathf.CeilToInt(currentStm)} / {Mathf.CeilToInt(maxStm)}";
        }
    }
}
