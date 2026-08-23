using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private PlayerSkill data;
    private float p_TotalDmg;
    private Vector3 casterPos;

    public void setUp(PlayerSkill skillData, float totalDmg, Vector3 casterPosition)
    {
        data = skillData;
        p_TotalDmg = totalDmg;
        casterPos = casterPosition;

        Destroy(gameObject, data.projectileLifeTime);
    }

    private void Update()
    {
        if (data == null) return;
        // Translate() : 유니티의 GameObject의 위치를 이동시키는 함수
        // transform.Translate(Vector3.forward * data.projectileSpeed * Time.deltaTime);
        transform.position += transform.forward * data.projectileSpeed * Time.deltaTime;

    }
    // OggerEnter(OnTriggerEnter) : 두 Collider가 Trigger영역에 겹쳤을때 자동 호출됨
    private void OnTriggerEnter(Collider other)
    {
        // other에 Enemy 컴포넌트 있으면 가져와서 enemy 변수에 넣음 (true/false)로 반환하기 때문에 사용 가능
        if (other.TryGetComponent(out Enemy enemy))
        {
            HitData hitData = data.CreateHitData(p_TotalDmg, casterPos);
            enemy.TakeDamage(hitData);

            Destroy(gameObject);
        }
    }
}
