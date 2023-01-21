using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    private UIManager uiManager;
    private PlayerController player;

    private StateManager state;
    private int enemyExperience;
    public float xpLevelIncreaseFactor;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("GameManager/UIManager").GetComponent<UIManager>();
    }

    private void Start()
    {
        state = gameManager.state;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void AddExperience(int xpValue)
    {
        state.PlayerTotalExperience += xpValue;
        state.PlayerCurrentLevelExperience += xpValue;
        if (state.PlayerCurrentLevelExperience > state.PlayerExperienceToNextLevel)
            LevelUp();
    }

    void LevelUp()
    {
        if (SoundControl.Instance != null)
            SoundControl.Instance.matchSoundControl.LevelUp();
        int experencieOvershot = state.PlayerCurrentLevelExperience - state.PlayerExperienceToNextLevel;
        state.PlayerCurrentLevelExperience = experencieOvershot;
        state.PlayerExperienceToNextLevel = (int)(xpLevelIncreaseFactor * state.PlayerExperienceToNextLevel);
        state.PlayerLevel += 1;

        // TODO: this will result in a couple minor logic bugs
        state.PlayerBaseDamage = (int)(state.PlayerBaseDamage * 1.1);
        state.PlayerCurrentDamage = state.PlayerBaseDamage;
        state.PlayerMaxHealth = (int)(state.PlayerMaxHealth * 1.1);
        state.PlayerHealth = state.PlayerMaxHealth;
        state.PlayerMaxMana = (int)(state.PlayerMaxMana * 1.1);
        state.PlayerMana = state.PlayerMaxMana;
        state.PlayerBaseResistance = (int)(state.PlayerBaseResistance * 1.1);
        state.PlayerCurrentResistance = state.PlayerBaseResistance;

        // update current and max values of sliders etc. and set values to full
        state.PlayerHealth = state.PlayerMaxHealth;
        state.PlayerMana = state.PlayerMaxMana;
        player.maxHealth = state.PlayerMaxHealth;
        player.maxMana = state.PlayerMaxMana;
        player.UpdateResourceBarsMaxValues();
        uiManager.UpdateUIPlayerResourceBarsMaxValues();

        if (state.PlayerLevel == 5)
        {
            Image abilitySlot4Image = GameObject.Find("IconAbilitySlot4").gameObject.transform.Find("AbilityImage").GetComponent<Image>();
            abilitySlot4Image.gameObject.SetActive(true);
        }
    }
}
