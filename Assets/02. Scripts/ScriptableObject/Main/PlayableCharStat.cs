using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharStat", menuName = "Scriptable Objects/PlayableCharStat")]
public class PlayableCharStat : ScriptableObject
{
    public string className = "클래스";
    public float maxHp = 100f;
    public float maxStamina = 100f;
    public float drainPerStamina = 10f;
    public float RegenPerStamina = 15f;
    public float walkSpeed = 10f;
    public float runSpeed = 18f;
    public float baseDmg = 10f;
    
}
