using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class FuriousChargeController : BaseAbilityController
{
    /* Written by Sebastian
     * 
     */
    [field: SerializeField]
    public override float CooldownTime { get; set; }
    public float SpriteDisplayTime;
    public float range;
    public int manaCost;
    public float abilityDamageFactor;
    public float abilityDuration;
    Timer abilityDurationTimer;
    Timer SpriteDisplayTimer;
    EnemyDetection enemyDetection;
    PlayerController playerController;
    NavMeshAgent agent;

    private UIManager uiManager;

    public override void Awake()
    {
        base.Awake();

        enemyDetection = GetComponent<EnemyDetection>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        abilityDurationTimer = new Timer(abilityDuration);
        abilityDurationTimer.TimerCompleteEvent += OnAbilityDurationCooldownTimerComplete;

        SpriteDisplayTimer = new Timer(SpriteDisplayTime);
        SpriteDisplayTimer.TimerCompleteEvent += OnSpriteDisplayTimerComplete;
    }

    public override void Start()
    {
        base.Start();
        state.Ability3State = AbilityState;

        AbilityState.Add("active", false);
        AbilityState.Add("manaCost", manaCost);
        AbilityState.Add("targetPosition", new Vector3(0f, 0f, 0f));
        AbilityState.Add("enemiesHit", new List<GameObject>());
        AbilityState.Add("duration", abilityDuration);
        AbilityState.Add("timeRemaining", 0f);
        agent = playerController.Agent;

        uiManager = GameObject.Find("GameManager/UIManager").GetComponent<UIManager>();
    }

    public override void Update()
    {
        base.Update();
        if (AbilityState["active"])
            DoDamage();

        if (SpriteDisplayTimer.IsActive)
            SpriteDisplayTimer.Update(Time.deltaTime);
        if (abilityDurationTimer.IsActive)
            abilityDurationTimer.Update(Time.deltaTime);
    }

    /* Written by Sebastian, edited by Daniela
     * 
     */
    public override void Channel()
    {
        if (Input.GetMouseButtonDown(0) && !AbilityState["active"] && !AbilityState["CooldownState"]["IsActive"] && state.PlayerMana >= manaCost)
        {
            Ray targetRay = playerController.mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(targetRay, out RaycastHit hit);
            AbilityState["targetPosition"] = hit.point;

            SetAgentChargeParameter(hit);

            SpriteRenderer.enabled = true;
            SpriteDisplayTimer.Start();
            abilityDurationTimer.Start();
            AbilityState["active"] = true;

            state.PlayerMana -= manaCost;
            if (SoundControl.Instance != null)
                SoundControl.Instance.matchSoundControl.Ability3();
            uiManager.HideChargeIndicator();

            base.Channel();
        }
        else
        {
            if (SoundControl.Instance != null)
            {
                SoundControl.Instance.matchSoundControl.AbilityNotUseable();
            }
        }
    }

    float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 difference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, difference) * sign;
    }

    void SetAgentChargeParameter(RaycastHit hit)
    {
        // TODO: these parameter should be accessed via player state
        agent.SetDestination(hit.point);
        agent.speed = 10 * 10;
        agent.acceleration = 60 * 3;
    }

    void ResetAgentChargeParameter()
    {
        agent.speed = 10;
        agent.acceleration = 60;
    }

    void DoDamage()
    {
        Collider[] enemiesInRange = enemyDetection.GetTeam1EnemiesInRange(playerController.transform.position, range);
        GameObject[] enemies = enemiesInRange.Select(enemyCollider => enemyCollider.gameObject).ToArray<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (!AbilityState["enemiesHit"].Contains(enemy))
            {
                AbilityState["enemiesHit"].Add(enemy);
                playerController.DoDamage(new GameObject[] { enemy }, abilityDamageFactor);  // This is ugyl but whatever
            }
        }

        AbilityState["timeRemaining"] = abilityDurationTimer.TimeRemaining;
        AbilityState["percentRemaining"] = abilityDurationTimer.PercentRemaining;
    }

    void OnAbilityDurationCooldownTimerComplete()
    {
        ResetAgentChargeParameter();
        AbilityState["enemiesHit"].Clear();
        // SpriteRenderer.enabled = false;
        AbilityState["active"] = false;
    }
    void OnSpriteDisplayTimerComplete()
    {
        SpriteRenderer.enabled = false;
        // SpriteTransform.SetParent(transform, true);
        // SpriteTransform.position = playerController.transform.position;
    }
}

