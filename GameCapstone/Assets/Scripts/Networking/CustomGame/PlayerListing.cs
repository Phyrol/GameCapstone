using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        gameObject.GetComponent<TextMeshProUGUI>().text = player.NickName;
    }
}
