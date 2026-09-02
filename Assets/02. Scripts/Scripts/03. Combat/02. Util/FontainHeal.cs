using System.Collections.Generic;
using UnityEngine;

public class PerHeal : MonoBehaviour
{
    [SerializeField] private float perHealAmount = 1f;
    private PlayerHP targetPlayer;
    private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        } 
        else if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHP>(out var hp))
            {
                targetPlayer = hp;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.RemoveWhere(enemy => enemy == null || !enemy.activeInHierarchy);
        }
        else if (other.CompareTag("Player"))
        {
            targetPlayer = null;
        }
    }
    private void Update()
    {
        if (targetPlayer == null) return;
        
        enemiesInRange.RemoveWhere(enemy => enemy == null || !enemy.activeInHierarchy);

        if (enemiesInRange.Count == 0)
        {
            targetPlayer.Heal(perHealAmount * Time.deltaTime);
        }
    }
    
}
