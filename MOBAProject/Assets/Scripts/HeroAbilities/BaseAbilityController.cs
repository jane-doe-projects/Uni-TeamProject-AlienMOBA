using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateUtils;

public abstract class BaseAbilityController : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    internal StateManager state;
    public abstract float CooldownTime { get; set; }
    internal Timer CooldownTimer { get; private set; }
    public Dictionary<string, dynamic> AbilityState;
    internal Transform SpriteTransform { get; private set; }
    internal SpriteRenderer SpriteRenderer { get; private set; }


    public virtual void Awake()
    {
        CooldownTimer = new Timer(CooldownTime);
        CooldownTimer.TimerCompleteEvent += OnCooldownTimerComplete;

        AbilityState = CreateBaseAbilityState();
    }

    public virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = gameManager.state;

        SpriteTransform = transform.Find("Sprite");
        SpriteRenderer = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer.enabled = false;
    }

    public virtual void Update()
    {
        if (CooldownTimer.IsActive)
        {
            CooldownTimer.Update(Time.deltaTime);
            SetStateCooldownUpdate();
        }
    }

    public virtual void Channel()
    {
        if (!CooldownTimer.IsActive)
        {
            CooldownTimer.Start();
            SetStateCooldownStart();
        }
    }

    void SetStateCooldownStart()
    {
        AbilityState["CooldownState"]["IsActive"] = true;
        AbilityState["CooldownState"]["TotalTime"] = CooldownTimer.TotalTime;
        AbilityState["CooldownState"]["TimeRemaining"] = CooldownTimer.TimeRemaining;
        AbilityState["CooldownState"]["TimeElapsed"] = CooldownTimer.TimeElapsed;
        AbilityState["CooldownState"]["PercentElapsed"] = CooldownTimer.PercentElapsed;
    }

    void SetStateCooldownUpdate()
    {
        AbilityState["CooldownState"]["TimeRemaining"] = CooldownTimer.TimeRemaining;
        AbilityState["CooldownState"]["TimeElapsed"] = CooldownTimer.TimeElapsed;
        AbilityState["CooldownState"]["PercentElapsed"] = CooldownTimer.PercentElapsed;
    }

    void SetStateCooldownComplete()
    {
        AbilityState["CooldownState"]["IsActive"] = false;
    }

    void OnCooldownTimerComplete()
    {
        SetStateCooldownComplete();
    }
}
