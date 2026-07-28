//TODO: 적 히트시 잠시 빨간색이 되는 로직 추가하기, 코드 리펙토링하기
using UnityEngine;

// 이 파일은 추상 클래스 -> 직접 객체 생성 불가, 다른 클래스가 상속받아 사용 (NormalEnemy,BossEnemy)
// protected -> 자기 자신 클래스 내부 or 상속받은 자식 클래스만 사용 가능
// virtual -> 이거 사용해서 함수 만들면 자식 클래스에서도 이 함수 재정의(Override) 가능
public abstract class Enemy : MonoBehaviour
{
    [Header("적 상태 수치")]
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;
    protected bool isDead = false;
    [Header("지면 체크 및 중력")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private float baseGravity = 1.5f;
    [SerializeField] private float hitRaiseGravity = 0.5f;

    private float currentGravityMul;
    private int hitCombo = 0;
    protected bool isGrounded;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody>();
        currentGravityMul = baseGravity;
    }
    protected virtual void Update()
    {
        if(groundCheck != null)
        {
            bool wasGrounded = isGrounded;
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer); // 바닥 착지 체크

            // 착지 시 공중 콤보 카운트 및 중력 가중치 리셋
            // 이 코드로 인해 딱 1프레임만 참이 되어 1번만 실행됨;
            if(isGrounded && !wasGrounded)
            {
                ResetGravity();
            }
        }
        else
            {
                // groundCheck없으면 RigidBody 속도 y축으로 임시 지면 체크
                isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f; 
            }
    }
    protected virtual void FixedUpdate()
    {
        ApplyCustomGravity(); // 낙하 중일시 커스텀중력 적용됨
    }
    private void ResetGravity()
    {
        hitCombo = 0;
        currentGravityMul = baseGravity;
    }
    /// <summary>
    /// 직접 정한 중력 가속도를 계산하는 로직
    /// </summary>
    private void ApplyCustomGravity()
    {
        if(isGrounded) return;

        if (rb.linearVelocity.y <= 0f)
        {
            Vector3 extraGravity = Physics.gravity * (currentGravityMul - 1f);
            rb.AddForce(extraGravity,ForceMode.Acceleration);
        }
    }
    /// <summary>
    /// 외부에서 데미지와 함께 공격한 플레이어의 위치 및 넉백 힘을 넘겨받는 함수
    /// </summary>
    public virtual void TakeDamage(float damage, Vector3 attackerPosition, float knockbackForce = 5f)
    {
        if (isDead) return;
        
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp,0f);

        Debug.Log($"[{gameObject.name}] 피격! 남은 체력 : {currentHp}/{maxHp}");

        ApplyKnockback(attackerPosition, knockbackForce);
        
        if (currentHp <= 0) Die();
    }
    // 기존 TakeDamage와의 호환성을 위해 남겨두는 기본 함수 (위치 미전달 시 넉백 없음)
    public virtual void TakeDamage(float damage)
    {
        TakeDamage(damage,transform.position, 0f);
    }

    /// <summary>
    /// 넉백 물리 힘 계산 및 적용
    /// </summary>
    protected virtual void ApplyKnockback(Vector3 attackerPosition, float force)
    {
        if (rb == null || force <= 0f) return;

        // 공격자의 위치에서 플레이어의 위치로 향하는 방향 벡터 계산 (Y축 높이는 0으로 해 수평 고정)
        Vector3 knockbackDir = transform.position - attackerPosition;
        knockbackDir.y = 0f;
        knockbackDir.Normalize();
        if(!isGrounded)
        {
            hitCombo++;
            currentGravityMul = baseGravity + (hitCombo * hitRaiseGravity);
            Debug.Log($"{hitCombo}Combo!");
        }

        // 이 두개의 변수로 완만한 포물선 형태로 날아가게 함
        float horizontalForce = force;
        float verticalForce = force * 0.7f;

        // Impulse를 사용해 순간적인 힘으로 넉백을 줌
        // ForceMode.Impulse => 오브젝트에 순간적인 힘을 주어 빠르게 속도를 바꿀 때 사용
        rb.linearVelocity = Vector3.zero; // 기존에 움직이던 속도를 0으로 리셋하여 더 깔끔하게 날아가게 함
        Vector3 finalForce = (knockbackDir * horizontalForce) + (Vector3.up * verticalForce);
        rb.AddForce(finalForce, ForceMode.Impulse);
    }
    protected virtual void Die()
    {
        isDead = true;
        Debug.Log("처치됨");
        Destroy(gameObject,0.2f);
    }
}
