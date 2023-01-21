using System.Collections;
using System.Collections.Generic;

public static class StateUtils
{
    /* Written by Sebastian
     * 
     */
    public static Dictionary<string, dynamic> CreateBaseAbilityState()
    {
        return new Dictionary<string, dynamic>(){
            { "CooldownState", CooldownState1() }
        };
    }

    public static Dictionary<string, dynamic> CooldownState1()
    {
        return new Dictionary<string, dynamic>(){
            {"TimeRemaining", 0f},
            {"TotalTime", 0f},
            {"IsActive", false},
            {"TimeElapsed", 0f},
            {"PercentElapsed", 0f},
        };
    }
}