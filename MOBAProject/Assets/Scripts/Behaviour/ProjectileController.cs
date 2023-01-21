using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    /* Written by Daniela on 20/07/2021
     * 
     */
    private GameObject targetEnemy;
    private int damageAmount;

    public static void CreateProjectile(Transform projectilePrefab, Vector3 spawnPosition, GameObject target, int damageAmount)
    {
        Transform projectileTransform = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        ProjectileController projectileController = projectileTransform.GetComponent<ProjectileController>();
        projectileController.SetTargetAndDamage(target, damageAmount);
    }

    private void SetTargetAndDamage(GameObject target, int damageAmount)
    {
        this.targetEnemy = target;
        this.damageAmount = damageAmount;
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            Vector3 targetPosition = targetEnemy.GetComponent<Collider>().bounds.center;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            float movementSpeed = 20f;
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if collision occured with intended target
        if (collision.gameObject.Equals(targetEnemy))
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damageAmount);
                // only play impact sound for impact on player hero
                player.PlayImpactSound();
            }
            else if (collision.gameObject.tag == "Creep")
            {
                MinionController creep = collision.gameObject.GetComponent<MinionController>();
                creep.TakeDamage(damageAmount);
            }
        }

        Destroy(gameObject);
    }
}
