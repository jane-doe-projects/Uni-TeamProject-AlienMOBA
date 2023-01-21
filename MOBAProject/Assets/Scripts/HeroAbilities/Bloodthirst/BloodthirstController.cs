using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Implement Bloodthirst Buff
// TODO: (Optional) Improve visuals
// TODO: Implement Buff indicator
public class BloodthirstController : BaseAbilityController
{
    /* Written by Sebastian
     * 
     */
    [field: SerializeField]
    public override float CooldownTime { get; set; }
    public float DamageBuff;
    public float ResistanceBuff;
    public float buffDuration;
    public float showEffectDuration;
    Timer buffTimer;
    Timer showEffectTimer;

    public override void Awake()
    {
        base.Awake();

        buffTimer = new Timer(buffDuration);
        buffTimer.TimerCompleteEvent += OnBuffCooldownTimerComplete;

        showEffectTimer = new Timer(showEffectDuration);
        showEffectTimer.TimerCompleteEvent += OnShowEffectCooldownTimerComplete;
    }

    public override void Start()
    {
        base.Start();
        state.Ability1State = AbilityState;

        AbilityState.Add("active", false);
        AbilityState.Add("type", "buff");
        AbilityState.Add("duration", buffDuration);
        AbilityState.Add("timeRemaining", 0f);
        AbilityState.Add("percentRemaining", 0f);
    }

    public override void Update()
    {
        base.Update();
        if (buffTimer.IsActive)
        {
            buffTimer.Update(Time.deltaTime);

            AbilityState["timeRemaining"] = buffTimer.TimeRemaining;
            AbilityState["percentRemaining"] = buffTimer.PercentRemaining;
        }
        if (showEffectTimer.IsActive)
        {
            SpriteTransform.localScale += new Vector3(0.001f, 0.001f, 0);
            float alphaValue = SpriteRenderer.color.a - (Time.deltaTime / showEffectDuration);
            SpriteRenderer.color = new Color(1f, 1f, 1f, alphaValue);
            showEffectTimer.Update(Time.deltaTime);
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
            DoBuff();
            showEffectTimer.Start();
            if (SoundControl.Instance != null)
                SoundControl.Instance.matchSoundControl.Ability1();
        } else
        {
            if (SoundControl.Instance != null)
            {
                SoundControl.Instance.matchSoundControl.AbilityNotUseable();
            }
        }
        base.Channel();
    }

    void DoBuff()
    {
        // TODO: this will lead to bugs
        state.PlayerCurrentDamage = state.PlayerBaseDamage * (1 + (DamageBuff / 100));
        state.PlayerCurrentResistance = state.PlayerBaseResistance * (1 + (ResistanceBuff / 100));
        buffTimer.Start();

        AbilityState["active"] = true;
        AbilityState["timeRemaining"] = buffTimer.TimeRemaining;
        AbilityState["percentRemaining"] = buffTimer.PercentRemaining;
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

    void OnBuffCooldownTimerComplete()
    {
        ResetBuff();
    }

    void OnShowEffectCooldownTimerComplete()
    {
        SpriteRenderer.enabled = false;
    }
}

