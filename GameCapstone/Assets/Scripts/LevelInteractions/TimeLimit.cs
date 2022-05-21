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
    public PhotonView timeView;

    private double time;
    private float limit = 60;

    public GameObject textObject;
    public TextMeshProUGUI timeLimit;

    private void Awake()
    {
        timeView = gameObject.GetComponent<PhotonView>();
        //gameObject.tag = "Wall";
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (timeView.IsMine)
        {
            Debug.Log("--Entered Start of Time limit--");

            //timePassed = 0;

            textObject = GameObject.Find("TimeLimit");
            if (textObject == null)
            {
                Debug.Log("--didn't find the time limit text--");
            }
            //timeLimit = textObject.GetComponent<TextMeshProUGUI>();

            //CreationTime = PhotonNetwork.Time;
        }

    }

    // Update is called once per frame
    public double theTime
    { // This is a property.
        get
        {
            return time;
        }
        set
        {
            time = value; // Note the use of the implicit variable "value" here.
        }
    }

    [PunRPC]
    public void Change()
    {
        //if (time == -1)
        //{
        //    timeLimit.text = "Closing In!!!";
        //    timeLimit.GetComponent<TextMeshProUGUI>().color = new Color32(230, 30, 30, 255);
        //}
        //else
        //{
        //    timeLimit.text = "Time Left: " + (Math.Ceiling(time)).ToString();
        //    timeLimit.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        //}
    }
    
}
