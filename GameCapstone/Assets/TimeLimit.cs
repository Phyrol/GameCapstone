using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
//using Math;

public class TimeLimit : MonoBehaviour
{
    public float passed;
    public float seconds;
    public float limit;

    public TextMeshProUGUI timeLimit;

    // Start is called before the first frame update
    void Start()
    {
        passed = 0;

        limit = 60;

        timeLimit = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        passed += Time.fixedDeltaTime;

        seconds = passed % 60;  
    }

    public void Change(double time)
    {
        if (time == -1)
        {
            timeLimit.text = "Closing In!!!";
            timeLimit.GetComponent<TextMeshProUGUI>().color = new Color32(230, 30, 30, 255);
        }
        else
        {
            timeLimit.text = "Time Left: " + (Math.Ceiling(time)).ToString();
            timeLimit.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        }
    }
}
