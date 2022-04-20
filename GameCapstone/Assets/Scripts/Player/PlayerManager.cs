using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private string playerPrefab;

    [SerializeField]
    private string playerName;

    private Player _player;

    private void Awake()
    {
        _player = PhotonNetwork.LocalPlayer;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerCharacter(string character)
    {
        playerPrefab = character;
    }

    public string GetPlayerCharacter()
    {
        return playerPrefab;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        _player.NickName = name;
        //ExitGames.Client.Photon.Hashtable newProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        //newProperties[StringConstants.CustomProperties_PlayerName] = playerName;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}
