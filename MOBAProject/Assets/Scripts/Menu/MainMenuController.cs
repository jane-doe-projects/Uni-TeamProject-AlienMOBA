using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    public GameObject controls;
    public GameObject settings;

    public GameObject startButton;
    public GameObject settingsButton;
    public GameObject controlsButton;
    public GameObject exitButton;

    public Slider musicSlider;
    public Slider effectsSlider;

    private void Start()
    {
        if (SoundControl.Instance != null)
        {
            SetMusicSliderValue(SoundControl.Instance.musicVolumeValue);
            SetEffectsSliderValue(SoundControl.Instance.effectsVolumeValue);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (controls.activeSelf)
            {
                HideControls();
            }
            else if (settings.activeSelf)
            {
                HideSettings();
            }
            else
            {
                ExitApplication();
            }
        }
    }

    public void StartGame()
    {
        // load the game map 
        SceneManager.LoadScene("Map1");
    }

    public void ExitApplication()
    {
        SoundControl.Instance.SaveVolumeSettings();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowControls()
    {
        controls.SetActive(true);
    }

    private void HideControls()
    {
        controls.SetActive(false);
    }

    public void ShowSettings()
    {
        settings.SetActive(true);
    }

    private void HideSettings()
    {
        SoundControl.Instance.SaveVolumeSettings();
        settings.SetActive(false);
    }

    private void SetMusicSliderValue(float value)
    {
        musicSlider.value = value;
        musicSlider.onValueChanged.AddListener(SoundControl.Instance.SetMusicVolume);
    }

    private void SetEffectsSliderValue(float value)
    {
        effectsSlider.value = value;
        effectsSlider.onValueChanged.AddListener(SoundControl.Instance.SetEffectsVolume);
    }
}
