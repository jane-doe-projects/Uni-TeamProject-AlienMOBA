using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaneMovementBehaviour : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    private StateManager state;

    public Transform spawnPosition;
    public GameObject enemyBuildings;
    public GameObject ownBuildings;

    private GameObject core;
    private GameObject turret1;
    private GameObject turret2;

    private GameObject ownTurret1;
    private GameObject ownTurret2;
    private float passProximity = 8f;

    private bool passedFirst = false;
    private bool passedSecond = false;

    private bool passedFirstEnemy = false;
    private bool passedSecondEnemy = false;

    NavMeshAgent agent;

    /* Written by Sebastian edited by Daniela
     * 
     */
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = gameManager.state;

        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            spawnPosition = GameObject.Find("PlayerBuildings/Core/CreepSpawn").transform;
            ownBuildings = GameObject.Find("PlayerBuildings").gameObject;
            enemyBuildings = GameObject.Find("EnemyBuildings").gameObject;
        } else
        {
            spawnPosition = GameObject.Find("EnemyBuildings/Core/CreepSpawn").transform;
            ownBuildings = GameObject.Find("EnemyBuildings").gameObject;
            enemyBuildings = GameObject.Find("PlayerBuildings").gameObject;
        }

        core = enemyBuildings.transform.Find("Core").gameObject;
        turret1 = enemyBuildings.transform.Find("Turret1").gameObject;
        turret2 = enemyBuildings.transform.Find("Turret2").gameObject;

        ownTurret1 = ownBuildings.transform.Find("Turret1").gameObject;
        ownTurret2 = ownBuildings.transform.Find("Turret2").gameObject;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.ResetPath();
        agent.Warp(spawnPosition.position);
    }

    /* Written by Sebastian, edited by Daniela
     * 
     */
    public void MoveAlongLane()
    {
        if (!state.MatchEnded)
        {
            // move along own buildings
            if (!passedFirst)
            {
                agent.destination = ownTurret2.transform.position;
                if (Vector3.Distance(agent.destination, transform.position) < passProximity)
                    passedFirst = true;
            }
            else if (!passedSecond)
            {
                agent.destination = ownTurret1.transform.position;
                if (Vector3.Distance(agent.destination, transform.position) < passProximity - 2)
                    passedSecond = true;
            }
            else
            {
                // move to enemy buildings
                if (!turret1.GetComponent<TurretController>().isDestroyed || !passedFirstEnemy)
                {
                    agent.destination = turret1.transform.position;
                    if (Vector3.Distance(agent.destination, transform.position) < passProximity - 2)
                        passedFirstEnemy = true;
                }
                else if (!turret2.GetComponent<TurretController>().isDestroyed || !passedSecondEnemy)
                {
                    agent.destination = turret2.transform.position;
                    if (Vector3.Distance(agent.destination, transform.position) < passProximity)
                        passedSecondEnemy = true;
                }
                else if (!core.GetComponent<TurretController>().isDestroyed)
                {
                    agent.destination = core.transform.position;
                }
            }
        }
        else
        {
            // stop creep movement on match end
            StopAgent();
        }

    }

    /* Written by Daniela
     * 
     */
    public void MoveToTarget(GameObject target)
    {
        if (!state.MatchEnded)
        {
            if (target != null)
            {
                agent.destination = target.transform.position;
            }
        } else
        {
            StopAgent();
        }
    }

    /* Written by Daniela
     * 
     */
    public void StopAgent()
    {
        agent.isStopped = true;
    }

    /* Written by Daniela
     * 
     */
    public void StartAgent()
    {
        agent.isStopped = false;
    }
}
