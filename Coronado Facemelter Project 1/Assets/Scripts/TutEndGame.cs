using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutEndGame : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject endScreen;
    public Text scoreCount;
    // Start is called before the first frame update
    void Start()
    {
        playPanel.SetActive(true);
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        EndGame();
    }

    public void EndGame()
    {
        if(scoreCount.text == "Score: 12")
        {
            endScreen.SetActive(true);
            playPanel.SetActive(false);
        }
        else
        {
            playPanel.SetActive(true);
            endScreen.SetActive(false);
        }
    }

    public void EndGameButton()
    {
        endScreen.SetActive(true);
        playPanel.SetActive(false);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
