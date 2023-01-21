using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    /* Written by Sebastian and Daniela
     * 
     */
    LaneMovementBehaviour laneMovementBehaviour;
    EnemyDetection enemyDetection;
    [SerializeField]
    private float detectionRadius;
    private float attackRadius;
    [SerializeField]
    private bool isEnemyCreep;

    private float autoAttackTimer;
    private float maxAutoAttackTimer = 1f;
    public int damageAmount;

    public int maxHealth;
    public int receivableXp = 80;
    [SerializeField]
    private int health;
    public ResourceBar healthBar;
    public bool isDead = false;

    private UIManager uiManager;
    private GameManager gameManager;
    private StateManager state;

    private Collider minionCollider;

    private MinionAnimationController minionAnimationController;

    private Collider currentTarget;

    void Awake()
    {
        health = maxHealth;
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        enemyDetection = GetComponent<EnemyDetection>();
        laneMovementBehaviour = GetComponent<LaneMovementBehaviour>();

        minionCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        minionAnimationController = GetComponent<MinionAnimationController>();
        state = gameManager.state;
        InitState();
        InitializeResourceBar();
    }

    void Update()
    {
        if (!isDead)
        {
            if (!state.MatchEnded)
            {
                if (!HandleEnemyDetectionAndAttack())
                {
                    laneMovementBehaviour.MoveAlongLane();
                }
            }
        }
    }

    void InitState()
    {
        state.CreepBaseHealth = maxHealth;
        state.CreepBaseDamage = damageAmount;
        state.CreepBaseKillXp = receivableXp;
    }

    private void InitializeResourceBar()
    {
        healthBar.slider.maxValue = maxHealth;
        healthBar.slider.value = maxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        if (!isDead)
        {
            health -= damageValue;
            if (health <= 0)
            {
                health = 0;
                HandleDeath();
            }
            UpdateLocalResourceBar();
        }
    }

    public void HandleDeath()
    {
        isDead = true;
        GetComponent<Targetable>().Disable();
        if (isEnemyCreep)
            gameManager.NotifyEnemyDeath(receivableXp);
        // disable collider so the creep does not get detected anylonger
        minionCollider.enabled = false;
        laneMovementBehaviour.StopAgent();

        StartCoroutine("DieAndDestroy");
    }

    IEnumerator DieAndDestroy()
    {
        minionAnimationController.TriggerDeathAnim();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void UpdateLocalResourceBar()
    {
        healthBar.slider.value = health;
    }

    private bool HandleEnemyDetectionAndAttack()
    {
        Collider[] enemiesInRange = enemyDetection.GetEnemiesInRange(transform.position, detectionRadius, isEnemyCreep);
        bool isInRangeAndAvailable = false;
        if (currentTarget != null)
            isInRangeAndAvailable = enemyDetection.IsInRange(currentTarget.gameObject, enemiesInRange);
        // if current target is still available, attack the current target
        if (isInRangeAndAvailable && enemyDetection.IsInMeleeRange(currentTarget.gameObject))
        {
            // stop agent and attack
            laneMovementBehaviour.StopAgent();
            transform.LookAt(currentTarget.transform.position);

            autoAttackTimer -= Time.deltaTime;
            if (autoAttackTimer <= 0f)
            {
                autoAttackTimer = maxAutoAttackTimer;
                AutoAttack(currentTarget.gameObject, damageAmount);
            }
        }
        else
        {
            // find new nearest target and assign it to current target
            Collider target = enemyDetection.GetClosestEnemy(transform.position, enemiesInRange);
            if (target != null)
            {
                if (enemyDetection.IsInMeleeRange(target.gameObject))
                    currentTarget = target;
                else
                {
                    laneMovementBehaviour.StartAgent();
                    laneMovementBehaviour.MoveToTarget(target.gameObject);
                }
            }
            else
            {
                laneMovementBehaviour.StartAgent();
                return false;
            }
        }
        return true;
    }

    private void AutoAttack(GameObject target, int damageAmount)
    {
        minionAnimationController.TriggerAutoAttackAnim();

        if (target.tag == "Player")
        {
            PlayerController player = target.GetComponent<PlayerController>();
            player.TakeDamage(damageAmount);
        }
        else if (target.tag == "Creep")
        {
            MinionController creep = target.GetComponent<MinionController>();
            creep.TakeDamage(damageAmount);
        }
        else if (target.tag == "Turret" || target.tag == "Core")
        {
            TurretController turret = target.GetComponent<TurretController>();
            turret.TakeDamage(damageAmount);
        }
    }

}
