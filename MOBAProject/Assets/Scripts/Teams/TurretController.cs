using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: state logic
public class TurretController : MonoBehaviour
{
    /* Written by Daniela on 22/07/2021
     * 
     */
    private UIManager uiManager;
    private GameManager gameManager;
    private StateManager state;

    private EnemyDetection enemyDetection;
    private Collider turretCollider;

    public bool isEnemyBuilding;

    public bool isCore;
    public float attackRadius;
    public int damageAmount = 250;
    public int receivableXp = 750;

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;

    public ResourceBar healthBar;
    public bool isDestroyed;

    public GameObject destructionPart;
    private Vector3 projectileOrigin;

    public Transform projectileOriginTransform;
    public Transform projectilePrefab;

    private float shotTimer;
    private float maxShotTimer;

    private Collider currentTarget;

    public AudioSource shotAudio;
    public AudioSource destructionAudio;

    private void Awake()
    {
        enemyDetection = GetComponent<EnemyDetection>();
        if (enemyDetection == null)
        {
            Debug.Log(gameObject.name + " is missing an EnemyDetection Component.");
        }

        turretCollider = GetComponent<CapsuleCollider>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        maxShotTimer = 1f;
        if (isCore)
            maxHealth = 20000;

        if (!isCore)
        {
            // only turrets can shoot and therefore need an origin for the projectile
            projectileOrigin = projectileOriginTransform.position;
        }

    }

    private void Start()
    {
        state = gameManager.state;
        InitializeResources();
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.MatchEnded)
        {
            if (!isDestroyed)
            {
                // destroy health on button press - for testing
                if (Input.GetKeyDown(KeyCode.S))
                {
                    TakeDamage(300);
                }

                HandleProtection();
            }
        }
    }

    private void HandleProtection()
    {
        Collider[] enemiesInRange = enemyDetection.GetEnemiesInRange(transform.position, attackRadius, isEnemyBuilding);
        bool currentStillInRangeAndAvailable = false;
        if (currentTarget != null)
            currentStillInRangeAndAvailable = enemyDetection.IsInRange(currentTarget.gameObject, enemiesInRange);
        // if current target is still available, attack the current target
        if (currentStillInRangeAndAvailable)
        {
            // turn turret towards / with target
            transform.LookAt(currentTarget.gameObject.transform.position);
            // update projectile origin due to rotation
            projectileOrigin = projectileOriginTransform.position;

            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0f)
            {
                shotTimer = maxShotTimer;
                // shoot projectile
                shotAudio.PlayOneShot(shotAudio.clip, 0.8f);
                ProjectileController.CreateProjectile(projectilePrefab, projectileOrigin, currentTarget.gameObject, damageAmount);
            }
        }
        else
        {
            // find new target
            Collider target = enemyDetection.GetClosestEnemy(transform.position, enemiesInRange);

            if (target != null)
                currentTarget = target;
        }

    }

    private void DefeatTurret()
    {
        isDestroyed = true;

        if (isEnemyBuilding)
        {
            GetComponent<Targetable>().Disable();
            gameManager.NotifyEnemyDeath(receivableXp);
        }
            
        if (isCore)
        {
            // end match
            gameManager.state.MatchEnded = true;
            if (isEnemyBuilding)
            {
                if (SoundControl.Instance != null)
                    SoundControl.Instance.matchSoundControl.Victory();
                uiManager.ShowEndPanel(isVictory: true);
                Debug.Log("Player won!");

            }
            else
            {
                if (SoundControl.Instance != null)
                    SoundControl.Instance.matchSoundControl.Defeat();
                uiManager.ShowEndPanel(isVictory: false);
                Debug.Log("Enemy won!");
            }
        }
        else
        {
            destructionAudio.PlayOneShot(destructionAudio.clip);
        }

        // TODO award XP to player / enemy

        // change visual of turret and deactivate functionaliy
        // deactivate gun part, health bar and collider
        turretCollider.enabled = false;
        destructionPart.SetActive(false);
        healthBar.gameObject.SetActive(false);

        // TODO VISUAL UPGRADE, add debris on the ground
    }


    private void InitializeResources()
    {
        // initialize turret resources and set healthbar
        health = maxHealth;
        healthBar.slider.maxValue = maxHealth;
        healthBar.slider.value = maxHealth;
    }

    private void UpdateLocalResourceBar()
    {
        healthBar.slider.value = health;
    }

    public void TakeDamage(int damageValue)
    {
        if (!isDestroyed)
        {
            health -= damageValue;
            if (health <= 0)
            {
                health = 0;
                DefeatTurret();
            }
            UpdateLocalResourceBar();
        }
    }
}
