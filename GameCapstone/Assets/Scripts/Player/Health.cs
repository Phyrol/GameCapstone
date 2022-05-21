using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public PhotonView view;

    public ParticleSystem bloodspray;
    public float StartingPercent;
    public bool MovementDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude >= 0 - Mathf.Epsilon && GetComponent<Rigidbody>().velocity.magnitude <= 0 + Mathf.Epsilon && MovementDisabled)
        {
            MovementDisabled = false;
        }
    }

    [PunRPC]
    void MeleeDamage(Vector3 direction, float damage, int playerNum)
    {
        int myPlayerNum = GetLocalActorNum();

        if (playerNum == myPlayerNum && gameObject.layer != 6)
        {
            GetComponentInChildren<Animator>().SetTrigger("isPunched");
            FindObjectOfType<AudioManager>().Play("Damage");

            bloodspray.Clear();
            bloodspray.Play();

            StartCoroutine(EmitTrail());

            GetComponent<Rigidbody>().AddForce(direction * (1 + StartingPercent / 100.0f), ForceMode.Impulse);
            Debug.Log($"DAMAGED: {direction * (1 + StartingPercent / 100.0f)}");

            StartingPercent += damage;

            ExitGames.Client.Photon.Hashtable newProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            newProperties[StringConstants.CustomProperties_PlayerHealth] = StartingPercent;
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);

            //Debug.Log($"{PhotonNetwork.LocalPlayer.ActorNumber}");
            LevelUIHandler.instance.UpdateHealth(myPlayerNum);
            //PhotonView.Get(LevelUIHandler.instance).RPC("UpdateHealth", RpcTarget.All, myPlayerNum);

            //HealthDisplay.Instance.SetHealthDisplay(StartingPercent);

            StartCoroutine(KnockbackStun());
        }
    }

    [PunRPC]
    void EnvironmentDamage(float damage, int playerNum)
    {
        int myPlayerNum = GetLocalActorNum();

        if(myPlayerNum == playerNum)
        {
            StartingPercent += damage;

            ExitGames.Client.Photon.Hashtable newProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            newProperties[StringConstants.CustomProperties_PlayerHealth] = StartingPercent;
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);

            //Debug.Log($"My player number: {PhotonNetwork.LocalPlayer.ActorNumber}");

            LevelUIHandler.instance.UpdateHealth(myPlayerNum);

            //PhotonView.Get(LevelUIHandler.instance).RPC("UpdateHealth", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);

            //HealthDisplay.Instance.SetHealthDisplay(StartingPercent);
        }
    }

    [PunRPC]
    void Dead(int playerNum)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == playerNum)
        {
            GetComponentInChildren<Animator>().SetTrigger("isDead");
            FindObjectOfType<AudioManager>().Play("Death");

            // temp way to kill player
            gameObject.GetComponent<PlayerController>().enabled = false;
            gameObject.GetComponentInChildren<Melee>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // end temp

            ExitGames.Client.Photon.Hashtable newProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            newProperties[StringConstants.CustomProperties_BoolPlayerAlive] = StringConstants.CustomProperties_PlayerIsDead;
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);

            PhotonView.Get(GameManager.instance).RPC("UpdatePlayerAliveCount", RpcTarget.All);

            LevelUIHandler.instance.ShowLoseScreen(playerNum);
            Debug.Log("I AM DEAD");
        }
    }

    void AddDamage(float damage)
    {
        StartingPercent += damage;
        HealthDisplay.Instance.SetHealthDisplay(StartingPercent);
    }

    int GetLocalActorNum()
    {
        return PhotonNetwork.LocalPlayer.ActorNumber;
    }

    IEnumerator EmitTrail()
    {
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        trail.emitting = true;
        yield return new WaitForSeconds(2f);
        trail.emitting = false;
    }

    IEnumerator KnockbackStun()
    {
        MovementDisabled = true;
        GetComponent<CapsuleCollider>().material.bounciness = 1;
        GetComponent<PlayerController>().useGravity = false;
        yield return new WaitForSeconds(StartingPercent/60.0f);
        MovementDisabled = false;
        GetComponent<CapsuleCollider>().material.bounciness = 0;
        GetComponent<PlayerController>().useGravity = true;
    }
}
