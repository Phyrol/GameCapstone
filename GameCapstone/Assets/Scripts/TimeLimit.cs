using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
//using Math;

public class TimeLimit : MonoBehaviour
{
    private PhotonView view;

    private float passed;
    public float seconds;
    private float limit = 60;

    public GameObject textObject;
    public TextMeshProUGUI timeLimit;

    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (view.IsMine)
        {
            Debug.Log("--Entered Start of Time limit--");

            passed = 0;

            textObject = GameObject.Find("TimeLimit");
            if (textObject == null)
            {
                Debug.Log("--didn't find the time limit text--");
            }
            timeLimit = textObject.GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
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
