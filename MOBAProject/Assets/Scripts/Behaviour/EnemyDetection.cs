using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    /* Written by Daniela on 23/07/2021
     * 
     */
    LayerMask enemyMask;
    LayerMask playerMask;

    private float meleeRange;

    private void Awake()
    {
        enemyMask = LayerMask.GetMask("Enemy");
        playerMask = LayerMask.GetMask("Player");
        meleeRange = 1f;
    }

    /* Written by Sebastian
     * 
     * */
    public Collider[] GetTeam1EnemiesInRange(Vector3 myPosition, float detectionRadius)
    {
        return GetEnemiesInRange(myPosition, detectionRadius, false);
    }

    /* Written by Sebastian
     * 
     */
    public Collider[] GetTeam2EnemiesInRange(Vector3 myPosition, float detectionRadius)
    {
        return GetEnemiesInRange(myPosition, detectionRadius, true);
    }

    public Collider[] GetEnemiesInRange(Vector3 attackerPosition, float detectionRadius, bool callerIsTeam2)
    {
        LayerMask mask = enemyMask;
        if (callerIsTeam2)
            mask = playerMask;
        return Physics.OverlapSphere(attackerPosition, detectionRadius, mask);
    }

    public Collider GetClosestEnemy(Vector3 attackerPosition, Collider[] enemiesInRange)
    {
        Collider closestTarget = null;

        foreach (Collider target in enemiesInRange)
        {
            if (closestTarget == null)
            {
                closestTarget = target;
            }
            else
            {
                if (Vector3.Distance(attackerPosition, target.transform.position) < Vector3.Distance(attackerPosition, closestTarget.transform.position))
                {
                    closestTarget = target;
                }
            }

        }

        return closestTarget;
    }

    public bool IsInMeleeRange(GameObject target)
    {
        meleeRange = 1.5f;
        if (target.tag == "Player")
        {
            meleeRange += 1f;
        } else if (target.tag == "Turret")
        {
            meleeRange += 2f;
        } else if (target.tag == "Core")
        {
            meleeRange += 4f;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < meleeRange)
        {
            return true;
        }
        return false;
    }

    public bool IsInRange(GameObject target, Collider[] enemiesInRange)
    {
        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.gameObject == target)
                return true;
        }
        return false;
    }
}
