using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemySpec : ScriptableObject
{
    public string enemyName = "적";
    public float maxHp = 50f;
    public float moveSpeed = 3.5f;
    public float attackPower = 10f;

}
