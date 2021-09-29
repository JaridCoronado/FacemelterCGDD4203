using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    //public TapNote tapNote;
    public ScoreCounter scoreCounter;
    bool active = false;
    GameObject tap;
    public int tapScore;
    public bool createMode;
    public GameObject newNote;

    public Vector3 center; 

    // Update is called once per frame
    void Update()
    {
        if (createMode == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(newNote, Input.mousePosition, Quaternion.identity);
            }
        }
        else if (createMode == false)
        {
            if (Input.GetMouseButtonDown(0) && active)
            {
                scoreCounter.AddToScore(6);
                Destroy(tap);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        active = true;
        if (col.gameObject.tag == "Tap")
        {
            tap = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        active = false;
    }
}
