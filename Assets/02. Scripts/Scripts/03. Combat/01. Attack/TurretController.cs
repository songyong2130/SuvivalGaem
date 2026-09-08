using UnityEngine;
public enum State
{
    Idle,
    Attack
}
public class TurretController : MonoBehaviour
{
    [SerializeField] private TurretTargeter targeter;
    [SerializeField] private TurretRotator rotator;
    [SerializeField] private TurretShooter shooter;
    [SerializeField] private TurretStat stat;
    private State currentState = State.Idle;
    private Enemy enemy;


    private void Update()
    {
        switch (currentState)
        {
            case State.Idle :
                targeter.SearchTarget();
                enemy = targeter.currentTarget;
                if (enemy != null)
                {
                    currentState = State.Attack;
                }
                break;
            case State.Attack :
                if (enemy == null) 
                { 
                    currentState = State.Idle; 
                    break;
                }
                rotator.Rotate(enemy);
                if (rotator.IsAimed) shooter.Shoot();
                break;

        }
    }
}
