using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    private int playersAliveCount;

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
            newProperties[StringConstants.CustomProperties_BoolPlayerAlive] = StringConstants.CustomProperties_PlayerIsAlive;
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
        }
    }

    [PunRPC]
    public void UpdatePlayerAliveCount()
    {
        playersAliveCount = 0;
        List<Player> aliveList = new List<Player>();
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            ExitGames.Client.Photon.Hashtable newProperties = player.CustomProperties;
            string playerAlive = (string)newProperties[StringConstants.CustomProperties_BoolPlayerAlive];

            if(playerAlive == "true")
            {
                aliveList.Add(player);
                playersAliveCount++;
            }
        }

        if(playersAliveCount == 1)
        {
            LevelUIHandler.instance.ShowWinScreen(aliveList[0].ActorNumber);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerAliveCount();
    }
}
