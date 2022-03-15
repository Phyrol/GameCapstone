using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TheWall : MonoBehaviour
{
    [SerializeField] public Vector3 center;
    [SerializeField] public float speed;

    [SerializeField] public float moveTimer;
    [SerializeField] public float waitTime;

    [SerializeField] public float stopWidth;

    //private GameObject notif;

    void Start()
    {
         speed = 0.05f; //0.01f is good to me
        //quick notes: keep under 1
        
        float x = (float)(21.05);
        center = new Vector3(x, gameObject.transform.position.y, 0);
        
        moveTimer = 0f;
        waitTime = 2.0f;

        stopWidth = 110f;

        //notif = GameObject.Find("dead text");
    }

    // Update is called once per frame
    void Update()
    {
        ///*-------------stuttered movement------------------
        moveTimer += Time.deltaTime;
        if (moveTimer < waitTime)
        {
            moveStutter();
        }
        else if (moveTimer > (waitTime * 2))
        {
            moveTimer = 0;
        }
        //*/

        //-------------smooth movement--------------------
        //moveSmooth();


    }

    private void OnCollisionEnter(Collision other)
    {
        //check for player tag
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is dead!!!");
            //player id dead
            //Text text = notif.GetComponent<Text>();
            //text.enabled = true;

            other.gameObject.GetComponent<PhotonView>().RPC("Dead", RpcTarget.Others, new object[] { });
        }
        else
        {
            Debug.Log("Not Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //check for player tag
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is dead!!!");
            //player id dead
            //Text text = notif.GetComponent<Text>();
            //text.enabled = true;

            other.gameObject.GetComponent<PhotonView>().RPC("Dead", RpcTarget.Others, new object[] { });
        }
        else
        {
            Debug.Log("Not Player");
        }
    }

    private void moveStutter()
    {
        if (gameObject.transform.localScale.x < stopWidth)
        {
            float xscale = gameObject.transform.localScale.x + speed;
            gameObject.transform.localScale = new Vector3(xscale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }

    private void moveSmooth()
    {
        if (gameObject.transform.localScale.x < stopWidth)
        {
            float xscale = gameObject.transform.localScale.x + speed;
            gameObject.transform.localScale = new Vector3(xscale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }        
    }

}
