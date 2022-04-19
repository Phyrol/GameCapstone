using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hazard : MonoBehaviour
{
    //private Dictionary<GameObject, bool> environmentDamageTimers;
    private Dictionary<int, bool> environmentDamageTimers;

    [SerializeField]
    private float damageToPlayer = 1.0f;

    [SerializeField]
    private float damageTimer = 1.0f;

    WaitForSeconds waitTime;

    private void Start()
    {
        environmentDamageTimers = new Dictionary<int, bool>();
        waitTime = new WaitForSeconds(damageTimer);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int playerViewID = other.collider.GetComponent<PhotonView>().ViewID;
            AddPlayer(playerViewID);

            if (environmentDamageTimers.ContainsKey(playerViewID) && !environmentDamageTimers[playerViewID])
            {
                StartCoroutine(DamagePlayer(playerViewID));
            }
                

            //other.gameObject.GetComponent<PhotonView>().RPC("Damage", RpcTarget.Others, new object[] { });
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int playerViewID = collision.collider.GetComponent<PhotonView>().ViewID;
            RemovePlayer(playerViewID);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int playerViewID = other.GetComponent<Collider>().GetComponent<PhotonView>().ViewID;
            AddPlayer(playerViewID);

            if (environmentDamageTimers.ContainsKey(playerViewID) && !environmentDamageTimers[playerViewID])
                DamagePlayer(playerViewID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int playerViewID = other.GetComponent<Collider>().GetComponent<PhotonView>().ViewID;
            RemovePlayer(playerViewID);
        }
    }

    private void AddPlayer(int id)
    {
        if (environmentDamageTimers.ContainsKey(id))
            return;

        Debug.Log($"Adding: {id}");
        environmentDamageTimers.Add(id, false);
    }

    private void RemovePlayer(int id)
    {
        if (environmentDamageTimers.ContainsKey(id))
            environmentDamageTimers.Remove(id);
    }

    IEnumerator DamagePlayer(int id)
    {
        Debug.Log($"Damage: {id}");
        PhotonView.Find(id).gameObject.GetComponent<PhotonView>().RPC("EnvironmentDamage", RpcTarget.Others, new object[] { damageToPlayer, id });
        environmentDamageTimers[id] = true;

        yield return waitTime;

        environmentDamageTimers[id] = false;
    }
}
