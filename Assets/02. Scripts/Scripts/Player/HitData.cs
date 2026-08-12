using UnityEngine;

// struct(구조체) : 여러개의 변수를 하나의 묶음으로 다루는 값 타입
// struct는 값을 통째로 복사하기 때문에, 참조 타입인 class와 달리 값이 복사되어 전달됨
// HitData는 짧은 순간에만 사용하여 struct로 정의함
public struct HitData
{
    public float damage;
    public Vector3 attackerPosition;
    public float knockbackForce;
    public float stunDuration;

    // 이후 상태이상 추가 할때 여기에 변수 추가하면 됨

}
