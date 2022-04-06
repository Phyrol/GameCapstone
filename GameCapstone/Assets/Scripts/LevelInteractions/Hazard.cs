using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hazard : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //check for player tag
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<PhotonView>().RPC("Damage", RpcTarget.Others, new object[] { });
        }

    }
}
