using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private float damageToPlayer = 1.0f;

    [SerializeField]
    private float damageTimer = 1.0f;

    WaitForSeconds waitTime;

    bool damagedPlayer = false;

    private void Start()
    {
        waitTime = new WaitForSeconds(damageTimer);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int playerActorNum = other.gameObject.GetComponent<PhotonView>().OwnerActorNr;
            int hazardActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

            if(playerActorNum == hazardActorNum)
            {
                //Debug.Log($"Collided locally: {playerActorNum}");
                //Debug.Log($"Hazard number: {hazardActorNum}");
                if (!damagedPlayer)
                {
                    other.gameObject.GetComponentInChildren<Animator>().SetTrigger("isPunched");
                    FindObjectOfType<AudioManager>().Play("Damage");
                    other.gameObject.GetComponent<PhotonView>().RPC("EnvironmentDamage", RpcTarget.All, new object[] { damageToPlayer, playerActorNum });
                    StartCoroutine(DamagePlayerTimer());
                }
                    
            }

            //Debug.Log($"ACTOR NUM: {playerViewID}");
            //AddPlayer(playerViewID);

            ////other.gameObject.GetComponent<PhotonView>().RPC("EnvironmentDamage", RpcTarget.Others, new object[] { damageToPlayer, playerViewID});

            //if (environmentDamageTimers.ContainsKey(playerViewID) && !environmentDamageTimers[playerViewID])
            //{
            //    StartCoroutine(DamagePlayer(playerViewID));
            //}
                

            ////other.gameObject.GetComponent<PhotonView>().RPC("Damage", RpcTarget.Others, new object[] { });
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int playerActorNum = other.gameObject.GetComponent<PhotonView>().OwnerActorNr;
            int hazardActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

            if (playerActorNum == hazardActorNum)
            {
                //Debug.Log($"Collided locally: {playerActorNum}");
                //Debug.Log($"Hazard number: {hazardActorNum}");
                if (!damagedPlayer)
                {
                    other.gameObject.GetComponent<PhotonView>().RPC("EnvironmentDamage", RpcTarget.All, new object[] { damageToPlayer, playerActorNum });
                    StartCoroutine(DamagePlayerTimer());
                }

            }
        }
    }

    IEnumerator DamagePlayerTimer()
    {
        damagedPlayer = true;

        yield return waitTime;

        damagedPlayer = false;
    }
}
