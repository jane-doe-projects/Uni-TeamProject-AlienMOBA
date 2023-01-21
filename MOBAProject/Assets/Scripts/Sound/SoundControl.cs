using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public class SoundControl : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    public static SoundControl Instance;
    public ButtonSoundControl buttonSoundControl;
    public MatchSoundControl matchSoundControl;

    public float musicVolumeValue;
    public float effectsVolumeValue;

    private string settingsFileName = "/settingsfile.json";

    public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadVolumeSettings();

        buttonSoundControl = GameObject.Find("SoundControl/ButtonSounds").GetComponent<ButtonSoundControl>();
        matchSoundControl = GameObject.Find("SoundControl/MatchSounds").GetComponent<MatchSoundControl>();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        musicVolumeValue = volume;
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", volume);
        effectsVolumeValue = volume;
    }

    // persistance of sound settings over sessions
    [System.Serializable]
    class SaveSettings
    {
        public float musicVolume;
        public float effectsVolume;
    }

    public void SaveVolumeSettings()
    {
        SaveSettings settings = new SaveSettings();
        settings.musicVolume = musicVolumeValue;
        settings.effectsVolume = effectsVolumeValue;

        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.persistentDataPath + settingsFileName, json);
    }

    public void LoadVolumeSettings()
    {
        string path = Application.persistentDataPath + settingsFileName;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveSettings settings = JsonUtility.FromJson<SaveSettings>(json);

            musicVolumeValue = settings.musicVolume;
            effectsVolumeValue = settings.effectsVolume;
        }
    }
}
