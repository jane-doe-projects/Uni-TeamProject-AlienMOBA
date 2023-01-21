using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAutoAttack : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    public GameObject targetedEnemy;
    public float autoAttackSpeed;
    private float autoAttackTimer;
    public bool performAutoAttack = true;
    private Animator playerAnim;

    private UIManager uiManager;
    private StateManager state;

    public float meleeAttackRange;
    private PlayerController player;
    private NavMeshAgent playerAgent;
    private float addedRange;

    // Start is called before the first frame update
    void Start()
    {
        state = GameObject.Find("GameManager").GetComponent<GameManager>().state;
        player = GetComponent<PlayerController>();
        playerAnim = GameObject.Find("Player/Alien").GetComponent<Animator>();
        playerAgent = player.GetComponent<NavMeshAgent>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedEnemy != null)
        {
            if (targetedEnemy.tag == "Core") addedRange = 4;
            else if (targetedEnemy.tag == "Turret") addedRange = 1;
            else addedRange = 0;
            if (Vector3.Distance(player.transform.position, targetedEnemy.transform.position) > meleeAttackRange + addedRange)
            {
                playerAgent.SetDestination(targetedEnemy.transform.position);
                playerAgent.stoppingDistance = meleeAttackRange - 1;
            }
            else
            {
                Vector3 lookAtPosition = targetedEnemy.transform.position - transform.position;
                lookAtPosition.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookAtPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

                if (performAutoAttack)
                {
                    autoAttackTimer -= Time.deltaTime;
                    if (autoAttackTimer <= 0f)
                    {
                        autoAttackTimer = autoAttackSpeed;
                        StartCoroutine("DoMeleeAttack");
                    }
                }
            }
        }
    }

    IEnumerator DoMeleeAttack()
    {
        performAutoAttack = false;
        playerAnim.SetBool("basicAttack", true);
        ApplyDamage();
        yield return new WaitForSeconds(autoAttackSpeed);

        if (targetedEnemy == null)
        {
            playerAnim.SetBool("basicAttack", false);
            performAutoAttack = true;
        }
    }

    public void ApplyDamage()
    {
        if (targetedEnemy != null)
        {
            if (targetedEnemy.tag == "Creep")
            {
                MinionController creep = targetedEnemy.GetComponent<MinionController>();
                creep.TakeDamage((int)state.PlayerBaseDamage);
            }
            else if (targetedEnemy.tag == "Turret" || targetedEnemy.tag == "Core")
            {
                TurretController turret = targetedEnemy.GetComponent<TurretController>();
                turret.TakeDamage((int)state.PlayerBaseDamage);
            }
            uiManager.IndicateDamage((int)state.PlayerBaseDamage, targetedEnemy);
        }
        performAutoAttack = true;
    }
}
