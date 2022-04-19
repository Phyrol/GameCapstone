using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        if(PhotonNetwork.IsConnected)
        {
            ExitGames.Client.Photon.Hashtable newProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            newProperties[StringConstants.CustomProperties_PlayerHealth] = 0;
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
        }
    }
}
