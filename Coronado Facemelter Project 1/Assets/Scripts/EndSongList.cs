using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSongList : MonoBehaviour
{
    public GameObject endScene;
    public GameObject gameScene;
    public AudioSource sheildWall;
    public float startTimeDelay;
    // Start is called before the first frame update
    void Awake()
    {
        gameScene.SetActive(true);
        endScene.SetActive(false);
        sheildWall.PlayDelayed(startTimeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!sheildWall.isPlaying)
        {
            gameScene.SetActive(false);
            endScene.SetActive(true);
        }*/
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
