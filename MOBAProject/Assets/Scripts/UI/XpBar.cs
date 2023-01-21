using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    private StateManager state;
    public Slider slider;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        state = gameManager.state;
        slider.maxValue = state.PlayerExperienceToNextLevel;
        slider.value = state.PlayerCurrentLevelExperience;
    }

    void Update()
    {
        slider.value = state.PlayerCurrentLevelExperience;
        slider.maxValue = state.PlayerExperienceToNextLevel;
    }
}
