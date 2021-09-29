using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel, songSelectPanel, settingPanel, creditsPanel, songListPanel;

    [SerializeField]
    private SettingsMenu settings;

    public static float sliderVal;

    // Start is called before the first frame update
    void Awake()
    {
        mainMenuPanel.SetActive(true);
        songSelectPanel.SetActive(false);
        settingPanel.SetActive(false);
        creditsPanel.SetActive(false);
        songListPanel.SetActive(false);
        settings.LoadSettings();
    }

    public void ToSongSelect()
    {
        print("Opening Song Selection...");
        settings.SFXTestBtn();
        songSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ToSettingsMenu()
    {
        print("Opening Settings...");
        settings.SFXTestBtn();
        settingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void Quit()
    {
        print("Quiting Game..");
        Application.Quit();
    }

    public void SettingsToMain()
    {
        settings.SaveSettings();
        print("Opening main..");
        settings.SFXTestBtn();
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void MainToCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
        settings.SFXTestBtn();
    }

    public void CreditsToMain()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settings.SFXTestBtn();
    }

    public void SongToMain()
    {
        print("Opening Song Selection...");
        settings.SFXTestBtn();
        songSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void GoToSongList()
    {
        songListPanel.SetActive(true);
        songSelectPanel.SetActive(false);
    }

    public void SonglistToMain()
    {
        songListPanel.SetActive(false);
        songSelectPanel.SetActive(true);
    }
    //Scene changers
    public void LoadSheildWall()
    {
        SceneManager.LoadScene("Amon Amarth - Sheild Wall");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadDethklok()
    {
        SceneManager.LoadScene("Deth Klok - I Ejaculate Fire");
    }

    public void LoadSYouSuffer()
    {
        SceneManager.LoadScene("Napalm Death - You Suffer");
    }
}
