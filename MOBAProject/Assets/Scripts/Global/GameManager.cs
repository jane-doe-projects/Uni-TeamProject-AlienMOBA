using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    /* Written by Daniela, edited by Sebastian
     * 
     */
    public UIManager uiManager;
    public StateManager state;

    // the countdown / preparation time until the match starts
    public float countDownTimer;

    [SerializeField]
    private float creepWaveInterval;
    [SerializeField]
    private int creepWaveSize;
    public GameObject enemyCreepPrefab;
    public GameObject playerCreepPrefab;

    public bool useBuffedEnemyCreeps = false;
    public GameObject buffedEnemyCreepPrefab;

    GameObject playerCreeps;
    GameObject enemyCreeps;

    // reference to player and enemy game objects
    public PlayerController player;
    public EnemyController enemy;

    private void Awake()
    {
        state = new StateManager();
        // spawn creep waves for player and enemy every 30 seconds
        creepWaveInterval = 30;
        creepWaveSize = 7;
        playerCreeps = GameObject.Find("CreepWaves/PlayerCreeps");
        enemyCreeps = GameObject.Find("CreepWaves/EnemyCreeps");
    }

    // Start is called before the first frame update
    void Start()
    {
        state.StartOfMatch = Time.time + countDownTimer;
        InvokeRepeating("SpawnWaves", (creepWaveInterval / 2) + countDownTimer, creepWaveInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.MatchEnded)
        {
            UpdateTimer();
        }
        else
        {
            // stops new creep wave spawns
            CancelInvoke();
        }

    }

    private void UpdateTimer()
    {
        // update the elapsed match time and the timer shown in the ui
        state.ElapsedMatchTime = Time.time - state.StartOfMatch;
        string minutes = ((int)state.ElapsedMatchTime / 60).ToString("00");
        string seconds = (state.ElapsedMatchTime % 60).ToString("00");
        // start match once counter hits 0
        if (state.ElapsedMatchTime >= 0 && !state.MatchStarted)
        {
            state.MatchStarted = true;
            if (SoundControl.Instance != null)
                SoundControl.Instance.matchSoundControl.MatchStarted();
        }
        if (state.MatchStarted)
            state.MatchGameTimerText = minutes + ":" + seconds;
        else
        {
            seconds = ((state.ElapsedMatchTime % 60) * -1).ToString("00");
            state.MatchGameTimerText = "-" + minutes + ":" + seconds;
        }
    }

    private void SpawnWaves()
    {
        for (int i = 0; i < creepWaveSize; i++)
        {
            GameObject playerCreep = Instantiate(playerCreepPrefab);
            GameObject enemyCreep;
            if (useBuffedEnemyCreeps)
                enemyCreep = Instantiate(buffedEnemyCreepPrefab);
            else
                enemyCreep = Instantiate(enemyCreepPrefab);

            // clean up creeps and set them as children of the creep wave objects
            playerCreep.transform.SetParent(playerCreeps.transform);
            enemyCreep.transform.SetParent(enemyCreeps.transform);
        }
    }

    public void NotifyEnemyDeath(int xp)
    {
        player.AddExperience(xp);
    }

    public void SwitchBuffedCreepsState()
    {
        useBuffedEnemyCreeps = !useBuffedEnemyCreeps;
    }
}
