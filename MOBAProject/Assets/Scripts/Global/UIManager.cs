using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    private GameManager gameManager;
    private StateManager state;

    // UI Elements
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI deathTimerText;

    // death overlay elements
    public GameObject playerDeathOverlay;
    public GameObject playerDeathPanelOverlay;
    public GameObject enemyDeathOverlay;

    // reference to player and enemy resource bars on ui
    public ResourceBar ResourceBarPlayerHealth;
    public ResourceBar ResourceBarPlayerMana;

    public ResourceBar ResourcBarEnemyHealth;
    public ResourceBar ResourceBarEnemyMana;

    public GameObject damageIndicationPrefab;
    public Vector3 playerDamageOffset;
    public Vector3 turretDamageOffset;
    public Vector3 creepDamageOffset;
    public Vector3 coreDamageOffset;

    public GameObject exitPanel;
    public Toggle buffedCreepsToogle;
    // private bool state.UiExitPanelActive = false;

    public GameObject victoryPanel;
    public GameObject defeatPanel;

    public bool chargeIndicatorActive = false;
    public Canvas chargeCanvas;
    public Image buffIndication;
    public Image chargeIndication;
    public Image warcryIndication;
    public Image whirlwindIndication;
    public Transform player;
    Vector3 position;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = gameManager.state;

        DeactivateIndicators();
        InitializeUIPlayerResourceBars();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !chargeIndicatorActive)
            SwitchExitPanelState();
        else if (Input.GetKeyDown(KeyCode.Escape) && chargeIndicatorActive)
            HideChargeIndicator();
        HandleIndicatorRotation();

        UpdateUIResourceBars();
        if (state.PlayerIsDead)
        {
            ActivatePlayerDeathOverlays();
            string seconds = ((state.PlayerTimeOfDeath + state.PlayerDeathTimer) - Time.time).ToString("f0");
            deathTimerText.text = seconds;
        }
        else
        {
            DeactivatePlayerDeathOverlay();
        }
        gameTimerText.text = state.MatchGameTimerText;
    }
    void ActivatePlayerDeathOverlays()
    {
        playerDeathOverlay.SetActive(true);
        playerDeathPanelOverlay.SetActive(true);
    }
    void DeactivatePlayerDeathOverlay()
    {
        playerDeathOverlay.SetActive(false);
        playerDeathPanelOverlay.SetActive(false);
    }

    void UpdateUIResourceBars()
    {
        ResourceBarPlayerHealth.SetValue(state.PlayerHealth);
        ResourceBarPlayerMana.SetValue(state.PlayerMana);
    }

    void InitializeUIPlayerResourceBars()
    {
        ResourceBarPlayerHealth.SetMaxValue(state.PlayerMaxHealth);
        ResourceBarPlayerMana.SetMaxValue(state.PlayerMaxMana);
    }

    public void UpdateUIPlayerResourceBarsMaxValues()
    {
        ResourceBarPlayerHealth.SetMaxValue(state.PlayerMaxHealth);
        ResourceBarPlayerMana.SetMaxValue(state.PlayerMaxMana);
    }

    public void IndicateDamage(int damageValue, GameObject target)
    {
        if (target != null)
        {
            Vector3 objectOffset;
            if (target.gameObject.tag == "Player" || target.gameObject.tag == "Enemy")
                objectOffset = playerDamageOffset;
            else if (target.gameObject.tag == "Turret")
                objectOffset = turretDamageOffset;
            else if (target.gameObject.tag == "Creep")
                objectOffset = creepDamageOffset;
            else
                objectOffset = coreDamageOffset;

            GameObject damageIndication = Instantiate(damageIndicationPrefab, target.transform.position + objectOffset, Quaternion.identity, target.transform);
            damageIndication.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damageValue.ToString());
        }
    }

    public void SwitchExitPanelState()
    {
        if (!state.MatchEnded)
        {
            if (state.UiExitPanelActive)
            {
                exitPanel.SetActive(false);
                state.UiExitPanelActive = false;
            }
            else
            {
                exitPanel.SetActive(true);
                state.UiExitPanelActive = true;
            }
        }
    }

    public void ExitMatch()
    {
        // lose all progress and go back to main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowEndPanel(bool isVictory)
    {
        if (isVictory)
            victoryPanel.SetActive(true);
        else
            defeatPanel.SetActive(true);
    }

    public void HideChargeIndicator()
    {
        chargeIndicatorActive = false;
        chargeIndication.enabled = false;
    }

    public void ShowChargeIndicator()
    {
        chargeIndicatorActive = true;
        chargeIndication.enabled = true;
    }

    private void HandleIndicatorRotation()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

        Quaternion transformRotation = Quaternion.LookRotation(position - player.transform.position);
        transformRotation.eulerAngles = new Vector3(90, transformRotation.eulerAngles.y, transformRotation.eulerAngles.z);
        chargeCanvas.transform.rotation = Quaternion.Lerp(transformRotation, chargeCanvas.transform.rotation, 0f);
    }

    public void DeactivateIndicators()
    {
        chargeIndication.enabled = false;
        chargeIndicatorActive = false;

        buffIndication.enabled = false;
        warcryIndication.enabled = false;
        whirlwindIndication.enabled = false;
    }


}
