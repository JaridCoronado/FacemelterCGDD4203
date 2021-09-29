using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OopsYouMissed : MonoBehaviour
{
    public Text tapsMissed;
    public Text tapsMissedFinal;
    private int tapsMissCounted;
    // Start is called before the first frame update
    void Start()
    {
        tapsMissCounted = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MissedTaps();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);
        tapsMissCounted++;
    }

    public void MissedTaps()
    {
        tapsMissed.text = "Missed: " + tapsMissCounted;
        tapsMissedFinal.text = "Missed: " + tapsMissCounted;
    }
}
