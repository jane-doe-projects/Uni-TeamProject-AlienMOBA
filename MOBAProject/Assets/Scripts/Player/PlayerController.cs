using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    /* Written by Sebastian and Daniela
     * 
     */
    private GameManager gameManager;
    private UIManager uiManager;
    private XPManager xpManager;
    private StateManager state;

    private EnemyDetection enemyDetection;

    public PlayerAnimationController playerAnimationController;

    internal NavMeshAgent Agent { get; private set; }
    public KeyCode KeycodeAbilitySlot1 = KeyCode.Q;
    public KeyCode KeycodeAbilitySlot2 = KeyCode.W;
    public KeyCode KeycodeAbilitySlot3 = KeyCode.E;
    public KeyCode KeycodeAbilitySlot4 = KeyCode.R;
    public GameObject AbilitySlot1;
    internal BaseAbilityController AbilitySlot1Controller { set; get; }
    public GameObject AbilitySlot2;
    internal BaseAbilityController AbilitySlot2Controller { set; get; }
    public GameObject AbilitySlot3;
    internal BaseAbilityController AbilitySlot3Controller { set; get; }
    public GameObject AbilitySlot4;
    internal BaseAbilityController AbilitySlot4Controller { set; get; }
    public Camera mainCamera;
    public GameObject destinationMark;
    public Transform spawnPosition;

    public int maxMana = 100;
    public int maxHealth = 1000;

    private float healthRegenTick = 0.0f;
    private float manaRegenTick = 0.0f;
    public float baseDamage = 85f;
    public float baseResistance = 1.0f;
    public int baseExperience = 1000;
    private ResourceBar healthBarOnPlayer;
    private ResourceBar manaBarOnPlayer;

    private Collider playerCollider;
    public AudioSource impactSound;

    private int hitLayer;
    private PlayerAutoAttack autoAttackControl;

    public void Awake()
    {
        healthBarOnPlayer = GameObject.Find("PlayerHealthBar").GetComponent<ResourceBar>();
        manaBarOnPlayer = GameObject.Find("PlayerManaBar").GetComponent<ResourceBar>();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        AbilitySlot1Controller = AbilitySlot1.GetComponent<BaseAbilityController>();
        AbilitySlot2Controller = AbilitySlot2.GetComponent<BaseAbilityController>();
        AbilitySlot3Controller = AbilitySlot3.GetComponent<BaseAbilityController>();
        AbilitySlot4Controller = AbilitySlot4.GetComponent<BaseAbilityController>();

        playerCollider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // get reference to the game manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        xpManager = transform.Find("XPManager").GetComponent<XPManager>();
        state = gameManager.state;

        enemyDetection = GetComponent<EnemyDetection>();
        InitState();

        autoAttackControl = GetComponent<PlayerAutoAttack>();

        Agent = this.GetComponent<NavMeshAgent>();
        SetAgentSettings();

        if (Agent == null)
            Debug.LogError("NavMeshAgent missing for " + gameObject.name);

        ResetToSpawn();
        InitializeLocalResourceBars();
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.MatchEnded)
        {
            if (!state.MatchStarted)
            {
                // blocks player and enemy from moving off the starting point before the game begins (keeps resetting them to their respective spawn)
                ResetToSpawn();
            }
            else
                playerAnimationController.SetMatchStarted();
            if (!state.PlayerIsDead)
            {
                // Ability Casting
                if (Input.GetKeyDown(KeycodeAbilitySlot1))
                    AbilitySlot1Controller.Channel();   
                if (Input.GetKeyDown(KeycodeAbilitySlot2))
                    AbilitySlot2Controller.Channel();   
                if (Input.GetKeyDown(KeycodeAbilitySlot3) && !uiManager.chargeIndicatorActive)
                    uiManager.ShowChargeIndicator();
                if (Input.GetMouseButtonDown(0) && uiManager.chargeIndicatorActive)
                    AbilitySlot3Controller.Channel();
                if (state.PlayerLevel >= 5)
                    if (Input.GetKeyDown(KeycodeAbilitySlot4))
                        AbilitySlot4Controller.Channel();   
                
                if (!state.UiExitPanelActive)
                    HandleMovement(); // disable the ability to move when the exit panel is active
                HandleManaRegen();
                HandleHealthRegen();
                TestHpAndMana();
                UpdateLocalResourceBars();
            }
        }
    }

    void InitState()
    {
        state.PlayerHealth = maxHealth;
        state.PlayerMana = maxMana;
        state.PlayerMaxHealth = maxHealth;
        state.PlayerMaxMana = maxMana;
        state.PlayerBaseDamage = baseDamage;
        state.PlayerCurrentDamage = baseDamage;
        state.PlayerBaseResistance = baseResistance;
        state.PlayerCurrentResistance = baseResistance;
        state.PlayerExperienceToNextLevel = baseExperience;
    }

    void TestHpAndMana()
    {
        // destroy 10 mana when pressing M
        if (Input.GetKeyDown(KeyCode.M))
        {
            UseMana(10);
        }
        // destroy 100 health when pressing H
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(100);
        }
    }

    // TODO: this should be set via editor, otherwise it will be hard to debug
    void SetAgentSettings()
    {
        Agent.speed = 10;
        Agent.angularSpeed = 1080;
        Agent.acceleration = 60;
    }

    void ShowDestinationMark(RaycastHit hit)
    {
        destinationMark.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
    }

    void HandleMovement()
    {
        if (autoAttackControl.targetedEnemy != null)
        {
            if (autoAttackControl.targetedEnemy.GetComponent<PlayerAutoAttack>() != null)
            {
                if (!state.PlayerIsDead)
                {
                    autoAttackControl.targetedEnemy = null;
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (uiManager.chargeIndicatorActive)
                uiManager.HideChargeIndicator();
            Ray targetRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // do raycast and check if mouse is aimed at enemy object or location
            if (Physics.Raycast(targetRay, out hit))
            {
                if (hit.collider)
                    hitLayer = hit.collider.gameObject.layer;
                if (hitLayer != LayerMask.NameToLayer("Enemy"))
                {
                    autoAttackControl.targetedEnemy = null;
                    Agent.stoppingDistance = 0;
                    Agent.SetDestination(hit.point);
                }
                else if (hit.collider.GetComponent<Targetable>() != null)
                {
                    if (hitLayer == LayerMask.NameToLayer("Enemy"))
                    {
                        autoAttackControl.targetedEnemy = hit.collider.gameObject;
                        autoAttackControl.performAutoAttack = true;
                    }
                }
                else if (hit.collider.GetComponent<Targetable>() == null)
                {
                    autoAttackControl.targetedEnemy = null;
                    autoAttackControl.performAutoAttack = false;
                }
                ShowDestinationMark(hit);
            }

        }
    }

    void ResetToSpawn()
    {
        Agent.ResetPath();
        Agent.Warp(spawnPosition.position);
    }

    public void TakeDamage(int damageValue)
    {
        if (!state.PlayerIsDead)
        {
            state.PlayerHealth -= (int)(damageValue - state.PlayerCurrentResistance);
            if (state.PlayerHealth <= 0)
            {
                state.PlayerHealth = 0;
                HandleDeath();
            }
        }
    }


    public void DoDamage(GameObject[] enemies, float damageFactor = 1f)
    {
        // TODO: make damage as float
        int outgoingDamage = (int)state.PlayerCurrentDamage;
        if (damageFactor != 1f)
            outgoingDamage = (int)(damageFactor * outgoingDamage);
        foreach (GameObject enemy in enemies)
        {
            if (enemy.tag == "Creep")
            {
                MinionController creep = enemy.GetComponent<MinionController>();
                uiManager.IndicateDamage(outgoingDamage, enemy);
                creep.TakeDamage(outgoingDamage);
            }
            else if (enemy.tag == "Turret" || enemy.tag == "Core")
            {
                TurretController turret = enemy.GetComponent<TurretController>();
                uiManager.IndicateDamage(outgoingDamage, enemy);
                turret.TakeDamage(outgoingDamage);
            }
            else
            {
                Debug.Log("Unable to get correct enemy controller. Please fix");
            }
        }
    }

    public void AddExperience(int xpValue)
    {
        xpManager.AddExperience(xpValue);
    }

    void UseMana(int manaValue) { state.PlayerMana -= manaValue; }

    void HandleManaRegen()
    {
        if (Time.time - manaRegenTick > 1.0f)
        {
            manaRegenTick = Time.time;
            if (state.PlayerMana < maxMana)
            {
                state.PlayerMana++;
            }
        }

        if (state.PlayerMana <= 0)
        {
            state.PlayerMana = 0;
        }
    }

    void HandleHealthRegen()
    {
        if (Time.time - healthRegenTick > 2.0f)
        {
            healthRegenTick = Time.time;
            if (state.PlayerHealth < maxHealth)
            {
                state.PlayerHealth++;
            }
        }

        if (state.PlayerHealth <= 0)
        {
            state.PlayerHealth = 0;
        }
    }

    IEnumerator HandleRespawn()
    {
        // TODO: init own state reference
        float elapsedTime = gameManager.state.ElapsedMatchTime;

        // death timer based on level progress
        state.PlayerDeathTimer = state.PlayerLevel + 5;
        yield return new WaitForSeconds(state.PlayerDeathTimer);

        RespawnPlayer();
    }

    void HandleDeath()
    {
        if (SoundControl.Instance != null)
            SoundControl.Instance.matchSoundControl.Death();

        autoAttackControl.targetedEnemy = null;
        autoAttackControl.performAutoAttack = false;

        UpdateLocalResourceBars();
        // set death variable to true
        state.PlayerIsDead = true;
        state.PlayerTimeOfDeath = Time.time;
        //disable collider until respawn so the player does not get detected/hit while dead
        playerCollider.enabled = false;

        // reset destination
        Agent.ResetPath();

        Debug.Log("Player died.");
        // put player back on spawn
        StartCoroutine("WaitBeforeReset");
        StartCoroutine("HandleRespawn");

    }

    void RespawnPlayer()
    {
        if (SoundControl.Instance != null)
            SoundControl.Instance.matchSoundControl.Respawn();
        // change player state to alive and reset resources (health/ mana)
        // deactivate overlay over ui resource bar and entire screen
        state.PlayerIsDead = false;
        state.PlayerHealth = maxHealth;
        state.PlayerMana = maxMana;
        playerCollider.enabled = true;
    }

    void UpdateLocalResourceBars()
    {
        healthBarOnPlayer.slider.value = state.PlayerHealth;
        manaBarOnPlayer.slider.value = state.PlayerMana;
    }

    void InitializeLocalResourceBars()
    {
        healthBarOnPlayer.slider.maxValue = maxHealth;
        healthBarOnPlayer.slider.value = maxHealth;
        manaBarOnPlayer.slider.maxValue = maxMana;
        manaBarOnPlayer.slider.value = maxMana;
    }

    IEnumerator WaitBeforeReset()
    {
        float waitTime = 2f;
        yield return new WaitForSeconds(waitTime);
        // reset to spawn after death animation finished
        ResetToSpawn();
    }

    public void PlayImpactSound()
    {
        impactSound.PlayOneShot(impactSound.clip);
    }

    public void UpdateResourceBarsMaxValues()
    {
        healthBarOnPlayer.slider.maxValue = state.PlayerMaxHealth;
        manaBarOnPlayer.slider.maxValue = state.PlayerMaxMana;
        //set values to full
        healthBarOnPlayer.slider.value = state.PlayerMaxHealth;
        manaBarOnPlayer.slider.value = state.PlayerMaxMana;
    }
}
