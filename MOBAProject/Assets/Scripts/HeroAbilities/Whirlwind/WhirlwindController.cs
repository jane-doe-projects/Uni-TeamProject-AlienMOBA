using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WhirlwindController : BaseAbilityController
{
    /* Written by Sebastian
     * 
     */
    [field: SerializeField]
    public override float CooldownTime { get; set; }

    public float abilityDuration;
    public float range;
    public float DamageBuff;
    public float ResistanceBuff;
    public float dotInterval;
    public float abilityDamageFactor;
    Timer abilityDurationTimer;
    PlayerController playerController;
    EnemyDetection enemyDetection;

    public override void Awake()
    {
        base.Awake();
        enemyDetection = GetComponent<EnemyDetection>();

        abilityDurationTimer = new Timer(abilityDuration);
        abilityDurationTimer.TimerCompleteEvent += OnAbilityDurationCooldownTimerComplete;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public override void Start()
    {
        base.Start();
        state.Ability4State = AbilityState;

        AbilityState.Add("active", false);
        AbilityState.Add("type", "buff+aoE");
        AbilityState.Add("duration", abilityDuration);
        AbilityState.Add("timeRemaining", 0f);
        AbilityState.Add("percentRemaining", 0f);
    }

    public override void Update()
    {
        base.Update();
        if (abilityDurationTimer.IsActive)
        {
            SpriteTransform.position = playerController.transform.position;
            SpriteTransform.Rotate(0f, 0f, 5f);
            abilityDurationTimer.Update(Time.deltaTime);
            AbilityState["timeRemaining"] = abilityDurationTimer.TimeRemaining;
            AbilityState["percentRemaining"] = abilityDurationTimer.PercentRemaining;
        }
    }

    /* Written by Sebastian, edited by Daniela
     * 
     */
    public override void Channel()
    {
        if (!CooldownTimer.IsActive)
        {
            SpriteRenderer.enabled = true;
            SpriteTransform.SetParent(null, true);
            SpriteTransform.position = playerController.transform.position;
            DoBuff();
            abilityDurationTimer.Start();
            StartCoroutine("DoAoeDotDamage");
            if (SoundControl.Instance != null)
                SoundControl.Instance.matchSoundControl.Ability4();
        } else
        {
            if (SoundControl.Instance != null)
            {
                SoundControl.Instance.matchSoundControl.AbilityNotUseable();
            }
        }
        base.Channel();
    }


    IEnumerator DoAoeDotDamage()
    {
        while (abilityDurationTimer.IsActive)
        {
            Collider[] enemiesInRange = enemyDetection.GetTeam1EnemiesInRange(playerController.transform.position, range);
            yield return new WaitForSecondsRealtime(dotInterval);
            GameObject[] enemies = enemiesInRange.Select(enemyCollider => enemyCollider.gameObject).ToArray<GameObject>();
            playerController.DoDamage(enemies, abilityDamageFactor);
        }
    }

    void DoBuff()
    {
        // TODO: this will lead to bugs
        state.PlayerCurrentDamage = state.PlayerBaseDamage * (1 + (DamageBuff / 100));
        state.PlayerCurrentResistance = state.PlayerBaseResistance * (1 + (ResistanceBuff / 100));
        abilityDurationTimer.Start();

        AbilityState["active"] = true;
        AbilityState["timeRemaining"] = abilityDurationTimer.TimeRemaining;
        AbilityState["percentRemaining"] = abilityDurationTimer.PercentRemaining;
    }

    void ResetBuff()
    {
        // TODO: this will lead to bugs
        state.PlayerCurrentDamage = state.PlayerBaseDamage;
        state.PlayerCurrentResistance = state.PlayerBaseResistance;

        AbilityState["active"] = false;
        AbilityState["timeRemaining"] = 0f;
        AbilityState["percentRemaining"] = 0f;
    }

    void OnAbilityDurationCooldownTimerComplete()
    {
        ResetBuff();
        SpriteRenderer.enabled = false;
        SpriteTransform.SetParent(transform, true);
    }
}

