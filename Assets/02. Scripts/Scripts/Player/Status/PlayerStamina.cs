using UnityEngine;
using UnityEngine.Events;

public class PlayerStamina : MonoBehaviour
{
    [Header("스탯 에셋")]
    [SerializeField] private PlayableCharStat playerStat;
    [Header("스태미나 기본 설정")]
    // [SerializeField] private float StmRegenRate = 15f;
    [SerializeField] private float regenDelay = 1f;
    public float currentStm {get; private set;}
    public float maxStm => (playerStat != null) ? playerStat.maxStamina : 100f; 
    [Header("UI")]
    public UnityEvent<float,float> onStaminaChanged;
    private float regenTimer;

    void Awake()
    {
        if (playerStat != null)
        {
            currentStm = playerStat.maxStamina;
        }
        else
        {
            Debug.LogWarning("PlayableCharStat가 연결되지 않았습니다.");
            currentStm = 100f;
        }
    }
    void Start()
    {
        if (onStaminaChanged == null)
        {
            Debug.LogError("onStaminaChagend가 연결되지 않았습니다.");
            return;
        }
        onStaminaChanged.Invoke(currentStm,maxStm);
    }
    void Update()
    {
        RegenStamina();
    }
    public bool DrainStamina(float ratePerSecond) 
    {
        if (currentStm <= 0) return false;

        currentStm -= ratePerSecond * Time.deltaTime;
        currentStm = Mathf.Max(0,currentStm);
        regenTimer = regenDelay;

        onStaminaChanged?.Invoke(currentStm,maxStm);
        return true;
    }
    private void RegenStamina()
    {
        if(regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
            return;
        }
        if (currentStm < maxStm)
        {
            currentStm += playerStat.RegenPerStamina * Time.deltaTime;
            currentStm = Mathf.Min(maxStm,currentStm);
            onStaminaChanged?.Invoke(currentStm,maxStm);
        }
    }
}
