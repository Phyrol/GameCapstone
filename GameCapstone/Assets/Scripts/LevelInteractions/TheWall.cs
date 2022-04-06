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
    private float speed = 0.03f;
    private double moveTimer;
    private double waitTime;
    private double startTime = 3;

    public PhotonView view;
    public GameObject textObject;

    private TimeLimit script;
    private bool run;


    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    public void Start()
    {
        float x = (float)(21.05);
        center = new Vector3(x, gameObject.transform.localPosition.y, 0);

        textObject = GameObject.Find("TimeLimit");
        if (textObject == null)
        {
            Debug.Log("--didn't find the time limit text--");
        }
        script = gameObject.GetComponentInParent<TimeLimit>();


        if (view.IsMine)
        {
            moveTimer = 0;
            waitTime = 2;
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

    private void OnCollisionEnter(Collision other)
    {
        //check for player tag
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is dead!!!");

            //other.gameObject.GetComponent<PhotonView>().RPC("Dead", RpcTarget.Others, new object[] { });
        }
        else
        {
            Debug.Log("Not Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player is damaged!!!");
        //other.gameObject.GetComponent<PhotonView>().RPC("Damage", RpcTarget.Others, new object[] { });
    }

    [PunRPC]
    private void moveStutter()
    {
        //*/
        if (!(Mathf.Abs(gameObject.transform.position.x - center.x) > 90))
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, center, -speed);
        }
    }

}
