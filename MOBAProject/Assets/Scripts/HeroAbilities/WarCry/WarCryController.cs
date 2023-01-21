using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WarCryController : BaseAbilityController
{
    /* Written by Sebastian
     * 
     */
    [field: SerializeField]
    public override float CooldownTime { get; set; }

    public float abilityDuration;
    public float abilityDamageFactor;
    public float dotInterval;
    public float range;
    Timer abilityDurationTimer;
    PlayerController playerController;
    EnemyDetection enemyDetection;
    // [Range(0, 50)]
    // public int segments = 50;
    // [Range(0, 50)]
    // public float xradius = 8;
    // [Range(0, 50)]
    // public float yradius = 8;
    // LineRenderer line;


    // void CreatePoints()
    // {
    //     float x;
    //     float y;
    //     float z;

    //     float angle = 20f;

    //     for (int i = 0; i < (segments + 1); i++)
    //     {
    //         x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
    //         y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

    //         line.SetPosition(i, new Vector3(x, y, 0));

    //         angle += (360f / segments);
    //     }
    // }

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
        // line = gameObject.GetComponentInChildren<LineRenderer>();

        // line.SetVertexCount(segments + 1);
        // line.useWorldSpace = false;
        // CreatePoints();
        state.Ability2State = AbilityState;

        AbilityState.Add("active", false);
        AbilityState.Add("duration", abilityDuration);
        AbilityState.Add("timeRemaining", 0f);
    }

    public override void Update()
    {
        base.Update();
        if (abilityDurationTimer.IsActive)
        {
            // Visuals
            // The rotation is actually around the y-axis. Just don't fiddle with it.
            SpriteTransform.Rotate(0f, 0f, 0.1f, Space.Self);
            float redValue = SpriteRenderer.color.r - (Time.deltaTime / abilityDuration);
            SpriteRenderer.color = new Color(redValue, 0f, 0f, 1f);

            // Logic
            DoAoeDotDamage();
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
            // Decouple the sprite from its ability to position it in the global space for a "permanent" effect
            SpriteTransform.SetParent(null, true);
            SpriteTransform.position = playerController.transform.position;
            SpriteRenderer.enabled = true;
            abilityDurationTimer.Start();
            AbilityState["active"] = true;

            StartCoroutine("DoAoeDotDamage");
            if (SoundControl.Instance != null)
                SoundControl.Instance.matchSoundControl.Ability2();
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
        Vector3 castPosition = transform.position;
        while (abilityDurationTimer.IsActive)
        {
            Collider[] enemiesInRange = enemyDetection.GetTeam1EnemiesInRange(castPosition, range);
            yield return new WaitForSecondsRealtime(dotInterval);
            GameObject[] enemies = enemiesInRange.Select(enemyCollider => enemyCollider.gameObject).ToArray<GameObject>();
            playerController.DoDamage(enemies, abilityDamageFactor);
        }
    }

    void OnAbilityDurationCooldownTimerComplete()
    {
        SpriteRenderer.enabled = false;
        AbilityState["active"] = false;
        SpriteTransform.SetParent(transform, true);
        SpriteTransform.position = playerController.transform.position;
        SpriteRenderer.color = new Color(1f, 0f, 0f, 1f);
    }
}

