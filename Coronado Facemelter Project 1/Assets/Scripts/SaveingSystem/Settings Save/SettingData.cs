using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public float musicVolume;
    public float soundEffectsVolume;
    public float musicSliderValue;
    public float soundSliderValue;

    public SettingData(SettingsMenu settings)
    {
        musicSliderValue = settings.musicSlider.value;
        soundSliderValue = settings.soundEffectsSlider.value;

        musicVolume = settings.mainMusic.volume;
        soundEffectsVolume = settings.hammerButton.volume;
    }
}
