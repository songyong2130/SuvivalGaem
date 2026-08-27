// TODO: EnemyHit 코드 다른 방향으로 구현 생각해보기 (색이 적용이 안됨) 지금 안쓸거임 ㅇㅇ
using System.Collections;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private Color hitColor = new Color(2f, 0.3f, 0.3f); 
    [SerializeField] private float hitDuration = 0.1f;

    private Renderer[] enemyRenderers;
    private MaterialPropertyBlock propBlock;
    
    // 두 가지 프로퍼티 ID 준비 (BaseColor와 EmissionColor)
    private int baseColorID;

    private void Awake()
    {
        enemyRenderers = GetComponentsInChildren<Renderer>();
        propBlock = new MaterialPropertyBlock();

        baseColorID = Shader.PropertyToID("_BaseColor");
        Debug.Log($"찾은 렌더러 개수: {enemyRenderers.Length}");
    }

    public void Hit()
    {
        Debug.Log("Hit() 함수 실행됨!");

        StopAllCoroutines();
        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        propBlock.SetColor(baseColorID, hitColor);

        foreach (Renderer rend in enemyRenderers)
        {
            rend.SetPropertyBlock(propBlock);
        }

        yield return new WaitForSeconds(hitDuration);

        foreach (Renderer rend in enemyRenderers)
        {
            rend.SetPropertyBlock(propBlock);
        }
    }
}