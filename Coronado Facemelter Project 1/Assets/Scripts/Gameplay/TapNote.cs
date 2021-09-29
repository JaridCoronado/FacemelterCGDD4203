using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapNote : MonoBehaviour
{
    public ScoreCounter scoreCounter;
    public GameObject tapNote;
    Rigidbody2D rb;
    public int scorePlus;
    public float fallSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(0, -fallSpeed);
    }
    public void TapButton()
    {
        scoreCounter.AddToScore(scorePlus);
        Destroy(this.gameObject);
    }
}
