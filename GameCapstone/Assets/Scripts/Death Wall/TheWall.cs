using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class TheWall : MonoBehaviour //PunCallbacks
{
    public double time;
    
    private Vector3 center;
    [SerializeField]
    private float speed = 0.03f; //0.03f
    private double moveTimer;
    private double waitTime;
    [SerializeField]
    private double startTime = 1; // was 30

    private PhotonView view;
    public GameObject textObject;

    private TimeLimit script;
    private bool run;

    private Vector3 start;


    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    public void Start()
    {
        float x = 21.05f;
        center = new Vector3(x, gameObject.transform.localPosition.y, 0);
        start = gameObject.transform.position;


        textObject = GameObject.Find("TimeLimit");
        if (textObject == null)
        {
            Debug.Log("--didn't find the time limit text--");
        }
        script = gameObject.GetComponentInParent<TimeLimit>();


        if (view.IsMine)
        {
            moveTimer = 0;
            waitTime = 20;
            run = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (view.IsMine) {
            moveTimer += Time.fixedDeltaTime;

            double initial = moveTimer % 60;
            double seconds = moveTimer % 60;

            if (initial < startTime && !run)
            {
                textObject.GetComponent<TextMeshProUGUI>().color = new Color32(30, 255, 30, 255);
            }
            else
            {
                run = true;
            }

            //-------------stuttered movement------------------//
            if (seconds < waitTime && run)
            {
                view.RPC("moveStutter", RpcTarget.All);
                script.theTime = -1;
                script.timeView.RPC("Change", RpcTarget.All);
            }
            else if (seconds > (waitTime * 2) && run)
            {
                moveTimer = 0;
            }
            else if (run)
            {
                script.theTime = (waitTime * 2) - seconds;
                script.timeView.RPC("Change", RpcTarget.All);
            }
        }

        transform.position = gameObject.transform.position;
    }

    [PunRPC]
    private void moveStutter()
    {
        if (Vector3.Distance(gameObject.transform.position, start) < 50f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, center, -speed);
        }
    }

}
