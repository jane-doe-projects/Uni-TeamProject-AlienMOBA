using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /* Written by Daniela
     * This script is mostly just a placeholder and was written/started in parallel with the player controller in the beginning. The enemy player is without function.
     */
    private GameManager gameManager;
    private StateManager state;
    public Transform spawnPosition;

    //private TargetManager targetManager;

    // Start is called before the first frame update
    void Start()
    {
        //targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
        // add enemy to enemyTargetList
        //targetManager.AddEnemyTarget(gameObject);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = gameManager.state;
        ResetToSpawn();
    }

    // Update is called once per frame
    void Update()
    {

        if (!state.MatchStarted) { ResetToSpawn(); }
    }

    public void ResetToSpawn()
    {
        // enemyAgent.ResetPath();
        // enemyAgent.Warp(spawnPosition.position);
        transform.position = spawnPosition.position;
    }
}
