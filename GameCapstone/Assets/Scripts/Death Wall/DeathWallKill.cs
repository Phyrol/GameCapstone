using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathWallKill : MonoBehaviour
{
    private PhotonView view;

    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is dead!!!");

            int playerNum = other.gameObject.GetComponent<PhotonView>().OwnerActorNr;
            other.gameObject.GetComponent<Health>().view.RPC("Dead", RpcTarget.All, new object[] { playerNum });
        }
        else
        {
            Debug.Log("Not Player");
        }
    }
}
