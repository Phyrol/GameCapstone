using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System;

public class TheWall : MonoBehaviour//PunCallbacks
{
    private Vector3 center;
    private float speed;
    private double moveTimer;
    private double waitTime;
    private double startTime = 3;

    public PhotonView view;
    public GameObject textObject;

    private TimeLimit script;
    private bool run;

    private void Awake()
    {
        view = gameObject.GetComponentInParent<PhotonView>();
    }
    public void Start()
    {
        if (view.IsMine)
        {
            float x = (float)(21.05);
            center = new Vector3(x, gameObject.transform.localPosition.y, 0); //center of map for now
            speed = 0.03f; //0.01f is good to me
                           //quick notes: keep under 1
            moveTimer = 0;
            waitTime = 2;
            run = false;
            //-----------------------------------------------
            textObject = GameObject.Find("TimeLimit");
            if (textObject == null)
            {
                Debug.Log("--didn't find the time limit text--");
            }
            script = gameObject.GetComponentInParent<TimeLimit>();
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
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
                moveStutter();
                script.Change(-1);
            }
            else if (seconds > (waitTime * 2) && run)
            {
                moveTimer = 0;
            }
            else if (run)
            {
                script.Change((waitTime * 2) - seconds);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //check for player tag
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is dead!!!");

            other.gameObject.GetComponent<PhotonView>().RPC("Dead", RpcTarget.Others, new object[] { });
        }
        else
        {
            Debug.Log("Not Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PhotonView>().RPC("Damage", RpcTarget.Others, new object[] { });
    }

    private void moveStutter()
    {
        /*
         * if(Vector3.Distance(center, gameObject.transform.localPosition) < 90)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(transform.localPosition, center, -speed);
        }
        */
        if (Vector3.Distance(center, gameObject.transform.position) < 90)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, center, -speed);
        }
    }

}
