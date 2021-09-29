using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text scoreCounter;
    public Text finalScore;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeScore();
    }

    public void AddToScore(int addScore)
    {
        score += addScore;
    }

    public void ChangeScore()
    {
        scoreCounter.text = "Score: " + score;
        finalScore.text = "Final Score: " + score;
    }
}
