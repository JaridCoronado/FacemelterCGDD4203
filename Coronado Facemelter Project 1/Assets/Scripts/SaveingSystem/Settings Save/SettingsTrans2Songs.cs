using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsTrans2Songs : MonoBehaviour
{
    [SerializeField] private AudioSource currentSong;
    [SerializeField] private SettingsMenu settings;
    // Start is called before the first frame update
    void Awake()
    {
        currentSong = settings.mainMusic;
        settings.LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
