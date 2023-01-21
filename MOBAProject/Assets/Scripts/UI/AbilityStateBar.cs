using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStateBar : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    private StateManager state;
    public string abilityName;
    public Slider slider;
    private Dictionary<string, dynamic> playerState;
    public Image background;
    public Image fill;


    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        state = gameManager.state;
        playerState = state.PlayerState;
        HideSlider();
    }

    void Update()
    {
        if (playerState[abilityName]["active"])
        {
            slider.maxValue = playerState[abilityName]["duration"];
            slider.value = playerState[abilityName]["timeRemaining"];
            ShowSlider();
        }
        else
            HideSlider();
    }

    void ShowSlider()
    {
        background.gameObject.SetActive(true);
        fill.gameObject.SetActive(true);
    }

    void HideSlider()
    {
        background.gameObject.SetActive(false);
        fill.gameObject.SetActive(false);
    }
}
