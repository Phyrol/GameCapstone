using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerList;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        int actorNum = player.ActorNumber - 1;
        _playerList.GetComponentsInChildren<TextMeshProUGUI>()[actorNum].text = player.NickName;
    }

    public void RemovePlayer(Player player)
    {
        int actorNum = player.ActorNumber - 1;
        _playerList.GetComponentsInChildren<TextMeshProUGUI>()[actorNum].text = "";
    }
}
