// TODO : if 게임이 커져 Enemy관리가 어렵다 ? => 바로 단일 책임원리로 나눔
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 이 파일은 추상 클래스 -> 직접 객체 생성 불가, 다른 클래스가 상속받아 사용 (NormalEnemy,BossEnemy)
// protected -> 자기 자신 클래스 내부 or 상속받은 자식 클래스만 사용 가능
// virtual -> 이거 사용해서 함수 만들면 자식 클래스에서도 이 함수 재정의(Override) 가능

public abstract class Enemy : MonoBehaviour
{
    [Header("적 상태 및 수치")]
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;
    protected bool isDead = false;
    public bool isStunned { get; private set;} = false;
    

    [Header("지면 체크 및 중력")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private float baseGravity = 1.5f;
    [SerializeField] private float hitRaiseGravity = 0.5f;

    private float currentGravityMul;
    private int hitCombo = 0;
    private Coroutine stunCoroutine;
    private Coroutine knockbackCoroutine;
    protected bool isGrounded;

    protected Rigidbody rb;
    protected NavMeshAgent agent;

    protected virtual void Awake()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        currentGravityMul = baseGravity;

        // 넉백 후 자연스럽게 멈추도록 기본값 보정
        if (rb != null) 
        {
            rb.isKinematic = true;
            if(rb.linearDamping == 0f) rb.linearDamping = 2f;
        }
    }

    protected virtual void Update()
    {
        bool wasGrounded = isGrounded;

        // 지면 체크 (groundCheck 설정 여부에 따른 예외 처리)
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        }
        else
        {
            // groundCheck가 없거나 isKinematic = True 시, Y 속도가 거의 없을 때를 지면으로 간주
            isGrounded = (rb != null && rb.isKinematic) || Mathf.Abs(rb.linearVelocity.y) < 0.05f;
        }

        // 착지 순간 (공중 -> 지상)에 정상적으로 콤보 및 중력 리셋
        if (isGrounded && !wasGrounded)
        {
            ResetGravity();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (agent != null && agent.enabled) return;
        ApplyCustomGravity();
    }
    private void ResetGravity()
    {
        hitCombo = 0;
        currentGravityMul = baseGravity;
    }

    private void ApplyCustomGravity()
    {
        if (isGrounded) return;

        if (rb.linearVelocity.y <= 0f)
        {
            Vector3 extraGravity = Physics.gravity * (currentGravityMul - 1f);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }

    // HitData 구조체 사용한 TakeDamage 넉백인지 스턴인지 판단한다.
    public virtual void TakeDamage(HitData hit)
    {
        if (isDead) return;
        
        currentHp -= hit.damage;
        currentHp = Mathf.Max(currentHp, 0f);

        Debug.Log($"[{gameObject.name}] 피격! 남은 체력 : {currentHp}/{maxHp}");

        if (hit.knockbackForce > 0f) 
        {
            float knockbackStun = Mathf.Max(0.5f,hit.stunDuration);
            ApplyKnockback(hit.attackerPosition, hit.knockbackForce,knockbackStun);
        } 
        else if (hit.stunDuration > 0f)
        {
            ApplyStun(hit.stunDuration);
        }
        if (currentHp <= 0) Die();

    }
    // 코루틴 전체 멈추게 하는 함 (넉백과 스턴 코루틴이 겹치면 스턴 지속 시간이 이상해지기 때문에)
    private void StopAllCC() {
        if (knockbackCoroutine != null) {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
        }
        if (stunCoroutine != null) {
            StopCoroutine(stunCoroutine);
            stunCoroutine = null;
        }
    }
    /// <summary>
    /// 넉백 처리를 담당하는 함수(넉백 시 경직이 있는 스킬이라면 IsStunned = true)
    /// </summary>
    #region KnockbackCoroutine
    protected virtual void ApplyKnockback(Vector3 attackerPosition, float force, float duration = 0f)
    {
        if (rb == null || force <= 0f) return;

        StopAllCC();

        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        hitCombo++; // 히트 콤보 가산을 먼저 앞으로 땡김
        if (!isGrounded)
        {
            currentGravityMul = baseGravity + (hitCombo * hitRaiseGravity);
            Debug.Log($"{hitCombo}Combo!");
        }
        knockbackCoroutine = StartCoroutine(KnockbackRoutine(attackerPosition, force, duration));
    }
    /// <summary>
    /// 넉백 계산을 해주는 함수
    /// </summary>
    private IEnumerator KnockbackRoutine(Vector3 attackerPosition, float force, float duration)
    {
        isStunned = true;

        // 넉백 시에는 agent 끄고 RigidBody 물리 킴
        if (rb != null) rb.isKinematic = false;
        if (agent != null) agent.enabled = false;

        Vector3 knockbackDir = transform.position - attackerPosition;
        knockbackDir.y = 0f;
        if (knockbackDir == Vector3.zero)
        {
            knockbackDir = transform.forward;
        }

        knockbackDir.Normalize();

        float horizontalForce = force;
        float verticalForce = force * 0.35f;

        rb.linearVelocity = Vector3.zero;
        Vector3 finalForce = (knockbackDir * horizontalForce) + (Vector3.up * verticalForce);
        rb.AddForce(finalForce, ForceMode.Impulse);

        yield return new WaitForSeconds(duration);
        // WaitUntil : 특정 조건이 true가 될때까지 코루틴을 잠시 기다리게 함
        yield return new WaitUntil(() => isGrounded); // 람다식

        // 넉백 끝나면 다시 원상 복구
        if (rb != null) rb.isKinematic = true;
        if (agent != null) agent.enabled = true;
        

        isStunned = false; // AI다시 활동
        knockbackCoroutine = null; // 코루틴을 비우는 이유? -> 실행 상태 확인 및 중복 실행이나 예외 막기위해
    }
    #endregion

    #region StunCoroutine
    protected virtual void ApplyStun(float duration)
    {
        StopAllCC();
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        stunCoroutine = StartCoroutine(StunRoutine(duration));
    }
    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;

        if (rb != null && isGrounded)
        {
            rb.linearVelocity = Vector3.zero;
        }

        yield return new WaitForSeconds(duration);

        isStunned = false;
        stunCoroutine = null;
    }
    #endregion

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log("처치됨");
        Destroy(gameObject, 0.2f);
    }
}