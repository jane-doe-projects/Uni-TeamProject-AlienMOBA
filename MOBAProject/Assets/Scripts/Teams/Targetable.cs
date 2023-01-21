using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    PlayerAutoAttack playerAutoAttack;

    private void Start()
    {
        playerAutoAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAutoAttack>();
    }
    public void Disable()
    {
        // set player target to null and disable component
        if (playerAutoAttack.targetedEnemy == gameObject)
        {
            playerAutoAttack.targetedEnemy = null;
        }
        enabled = false;
    }
}
