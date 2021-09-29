using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioSource hammerButton;
    public AudioSource mainMusic;

    public Slider musicSlider;
    public Slider soundEffectsSlider;

    private void Awake()
    {
        LoadSettings();
    }

    public void MusicSlider()
    {
        mainMusic.volume = musicSlider.value;
    }

    public void SoundEffectsSlider()
    {
        hammerButton.volume = soundEffectsSlider.value;
    }

    public void SFXTestBtn()
    {
        hammerButton.Play();
    }

    public void SaveSettings()
    {
        SettingsSaveSystem.SaveSettings(this);
    }

    public void LoadSettings()
    {
        SettingData data = SettingsSaveSystem.LoadData();

        hammerButton.volume = data.soundEffectsVolume;
        mainMusic.volume = data.musicVolume;
        musicSlider.value = data.musicSliderValue;
        soundEffectsSlider.value = data.soundSliderValue;
    }
}
