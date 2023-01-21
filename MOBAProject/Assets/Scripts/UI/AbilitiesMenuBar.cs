using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesMenuBar : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    private StateManager state;
    public GameObject player;
    PlayerController playerController;

    [Header("Ability 1")]
    public GameObject abilitySlot1UiIcon;
    GameObject abilitySlot1GameObject;

    Dictionary<string, dynamic> abilitySlot1State;
    Image abilitySlot1Image;

    [Header("Ability 2")]
    public GameObject abilitySlot2UiIcon;
    GameObject abilitySlot2GameObject;

    Dictionary<string, dynamic> abilitySlot2State;
    Image abilitySlot2Image;

    [Header("Ability 3")]
    public GameObject abilitySlot3UiIcon;
    GameObject abilitySlot3GameObject;

    Dictionary<string, dynamic> abilitySlot3State;
    Image abilitySlot3Image;

    [Header("Ability 4")]
    public GameObject abilitySlot4UiIcon;
    GameObject abilitySlot4GameObject;

    Dictionary<string, dynamic> abilitySlot4State;
    Image abilitySlot4Image;

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        abilitySlot1GameObject = playerController.AbilitySlot1;
        abilitySlot2GameObject = playerController.AbilitySlot2;
        abilitySlot3GameObject = playerController.AbilitySlot3;
        abilitySlot4GameObject = playerController.AbilitySlot4;

        // Ability Slot 1
        abilitySlot1Image = abilitySlot1UiIcon.transform.Find("AbilityImage").GetComponent<Image>();
        abilitySlot1Image.sprite = abilitySlot1GameObject.GetComponent<Image>().sprite;
        abilitySlot1Image.fillAmount = 1;
        abilitySlot1UiIcon.transform.Find("AbilityImageBackground").GetComponent<Image>().sprite = abilitySlot1Image.sprite;

        // Ability Slot 2
        abilitySlot2Image = abilitySlot2UiIcon.transform.Find("AbilityImage").GetComponent<Image>();
        abilitySlot2Image.sprite = abilitySlot2GameObject.GetComponent<Image>().sprite;
        abilitySlot2Image.fillAmount = 1;
        abilitySlot2UiIcon.transform.Find("AbilityImageBackground").GetComponent<Image>().sprite = abilitySlot2Image.sprite;

        // Ability Slot 3
        abilitySlot3Image = abilitySlot3UiIcon.transform.Find("AbilityImage").GetComponent<Image>();
        abilitySlot3Image.sprite = abilitySlot3GameObject.GetComponent<Image>().sprite;
        abilitySlot3Image.fillAmount = 1;
        abilitySlot3UiIcon.transform.Find("AbilityImageBackground").GetComponent<Image>().sprite = abilitySlot3Image.sprite;

        // Ability Slot 4
        abilitySlot4Image = abilitySlot4UiIcon.transform.Find("AbilityImage").GetComponent<Image>();
        abilitySlot4Image.sprite = abilitySlot4GameObject.GetComponent<Image>().sprite;
        abilitySlot4Image.fillAmount = 1;
        abilitySlot4UiIcon.transform.Find("AbilityImageBackground").GetComponent<Image>().sprite = abilitySlot4Image.sprite;
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = gameManager.state;
        abilitySlot1State = abilitySlot1GameObject.GetComponent<BaseAbilityController>().AbilityState;
        abilitySlot2State = abilitySlot2GameObject.GetComponent<BaseAbilityController>().AbilityState;
        abilitySlot3State = abilitySlot3GameObject.GetComponent<BaseAbilityController>().AbilityState;
        abilitySlot4State = abilitySlot4GameObject.GetComponent<BaseAbilityController>().AbilityState;

        abilitySlot4Image.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateAbilityIcon(abilitySlot1Image, abilitySlot1State);
        UpdateAbilityIcon(abilitySlot2Image, abilitySlot2State);
        if (abilitySlot3State["manaCost"] > state.PlayerMana)
            abilitySlot3Image.fillAmount = 0;
        else
            UpdateAbilityIcon(abilitySlot3Image, abilitySlot3State);
        UpdateAbilityIcon(abilitySlot4Image, abilitySlot4State);
    }

    void UpdateAbilityIcon(Image image, Dictionary<string, dynamic> abilityState)
    {
        if (abilityState["CooldownState"]["IsActive"])
            image.fillAmount = abilityState["CooldownState"]["PercentElapsed"];
        else
            image.fillAmount = 1;
    }
}
