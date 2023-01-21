using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    /* Written by Sebastian 
     * 
     */
    private Dictionary<string, dynamic> State = new Dictionary<string, dynamic>();

    public StateManager()
    {
        Dictionary<string, dynamic> gameDict = new Dictionary<string, dynamic>()
            {
                { "ElapsedMatchTime", 0f },
                { "StartOfMatch", 0f },
                { "MatchStarted", false },
                { "MatchEnded" , false },
                { "gameTimerText", "" },
            };
        State.Add("game", gameDict);

        Dictionary<string, dynamic> playerDict = new Dictionary<string, dynamic>()
            {
                { "health", 0 },
                { "maxHealth", 0 },
                { "mana", 0 },
                { "maxMana", 0 },
                { "baseDamage", 0f },
                { "currentDamage", 0f },
                { "baseResistance", 0f },
                { "currentResistance", 0f },
                { "buffs", new Dictionary<string, dynamic>(){} },
                { "timeOfDeath", 0f },
                { "deathTimer", 0f },
                { "isDead", false },
                { "totalExperience", 0 },
                { "level", 1 },
                { "experienceToNextLevel", 0 },
                { "currentLevelExperience", 0 },
                // { "ability1", new Dictionary<string, dynamic>(){} },
                // { "ability2", new Dictionary<string, dynamic>(){} },
                // { "ability3", new Dictionary<string, dynamic>(){} },
                // { "ability4", new Dictionary<string, dynamic>(){} },
            };
        State.Add("player", playerDict);

        Dictionary<string, dynamic> minionDict = new Dictionary<string, dynamic>()
            {
                { "baseDamage", 0 },
                { "baseHealth", 0 },
                { "baseKillXp", 0 },
            };
        State.Add("creep", minionDict);

        Dictionary<string, dynamic> uiDict = new Dictionary<string, dynamic>()
            {
                { "exitPanelActive", false },
            };
        State.Add("ui", uiDict);

        Dictionary<string, dynamic> settingsDict = new Dictionary<string, dynamic>()
            {
                { "cameraLockedToPlayer", true },
            };
        State.Add("settings", settingsDict);
    }

    public float ElapsedMatchTime
    {
        get => State["game"]["ElapsedMatchTime"];
        set => State["game"]["ElapsedMatchTime"] = value;
    }
    public float StartOfMatch
    {
        get => State["game"]["StartOfMatch"];
        set => State["game"]["StartOfMatch"] = value;
    }
    public bool MatchStarted
    {
        get => State["game"]["MatchStarted"];
        set => State["game"]["MatchStarted"] = value;
    }

    public bool MatchEnded
    {
        get => State["game"]["MatchEnded"];
        set => State["game"]["MatchEnded"] = value;
    }
    public string MatchGameTimerText
    {
        get => State["game"]["gameTimerText"];
        set => State["game"]["gameTimerText"] = value;
    }
    public int PlayerHealth
    {
        get => State["player"]["health"];
        set => State["player"]["health"] = value;
    }
    public int PlayerMaxHealth
    {
        get => State["player"]["maxHealth"];
        set => State["player"]["maxHealth"] = value;
    }
    public int PlayerMana
    {
        get => State["player"]["mana"];
        set => State["player"]["mana"] = value;
    }
    public int PlayerMaxMana
    {
        get => State["player"]["maxMana"];
        set => State["player"]["maxMana"] = value;
    }
    public float PlayerTimeOfDeath
    {
        get => State["player"]["timeOfDeath"];
        set => State["player"]["timeOfDeath"] = value;
    }
    public float PlayerDeathTimer
    {
        get => State["player"]["deathTimer"];
        set => State["player"]["deathTimer"] = value;
    }
    public bool PlayerIsDead
    {
        get => State["player"]["isDead"];
        set => State["player"]["isDead"] = value;
    }
    public int PlayerTotalExperience
    {
        get => State["player"]["totalExperience"];
        set => State["player"]["totalExperience"] = value;
    }
    public int PlayerLevel
    {
        get => State["player"]["level"];
        set => State["player"]["level"] = value;
    }
    public int PlayerExperienceToNextLevel
    {
        get => State["player"]["experienceToNextLevel"];
        set => State["player"]["experienceToNextLevel"] = value;
    }
    public int PlayerCurrentLevelExperience
    {
        get => State["player"]["currentLevelExperience"];
        set => State["player"]["currentLevelExperience"] = value;
    }
    public float PlayerBaseDamage
    {
        get => State["player"]["baseDamage"];
        set => State["player"]["baseDamage"] = value;
    }
    public float PlayerCurrentDamage
    {
        get => State["player"]["currentDamage"];
        set => State["player"]["currentDamage"] = value;
    }
    public float PlayerBaseResistance
    {
        get => State["player"]["baseResistance"];
        set => State["player"]["baseResistance"] = value;
    }
    public float PlayerCurrentResistance
    {
        get => State["player"]["currentResistance"];
        set => State["player"]["currentResistance"] = value;
    }
    public Dictionary<string, dynamic> PlayerBuffs
    {
        get => State["player"]["buffs"];
        set => State["player"]["buffs"] = value;
    }
    public Dictionary<string, dynamic> PlayerState
    {
        get => State["player"];
        set => State["player"] = value;
    }

    public Dictionary<string, dynamic> Ability1State
    {
        get => State["player"]["ability1"];
        set => State["player"]["ability1"] = value;
    }

    public Dictionary<string, dynamic> Ability2State
    {
        get => State["player"]["ability2"];
        set => State["player"]["ability2"] = value;
    }

    public Dictionary<string, dynamic> Ability3State
    {
        get => State["player"]["ability3"];
        set => State["player"]["ability3"] = value;
    }

    public Dictionary<string, dynamic> Ability4State
    {
        get => State["player"]["ability4"];
        set => State["player"]["ability4"] = value;
    }
    public bool UiExitPanelActive
    {
        get => State["ui"]["exitPanelActive"];
        set => State["ui"]["exitPanelActive"] = value;
    }
    public bool SettingsCameraLockedToPlayer
    {
        get => State["settings"]["cameraLockedToPlayer"];
        set => State["settings"]["cameraLockedToPlayer"] = value;
    }

    public int CreepBaseDamage
    {
        get => State["creep"]["baseDamage"];
        set => State["creep"]["baseDamage"] = value;
    }

    public int CreepBaseHealth
    {
        get => State["creep"]["baseHealth"];
        set => State["creep"]["baseHealth"] = value;
    }

    public int CreepBaseKillXp
    {
        get => State["creep"]["baseKillXp"];
        set => State["creep"]["baseKillXp"] = value;
    }
}
