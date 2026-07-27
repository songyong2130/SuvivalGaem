using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("상태 수치")]
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        currentHp = maxHp;
    }
    public virtual void takeDamage(float damage)
    {
        if (isDead) return;
        
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp,0f);

        Debug.Log($"[{gameObject.name}] 피격! 남은 체력 : {currentHp}/{maxHp}");
        
        if (currentHp <= 0) Die();
    }
    protected virtual void Die()
    {
        isDead = true;
        Debug.Log("처치됨");
        Destroy(gameObject,0.2f);
    }
}
