using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LevelUIHandler : MonoBehaviour
{
    public static LevelUIHandler instance;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private TextMeshProUGUI localPlayerHealthText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [PunRPC]
    public void UpdateHealth(int playerNum)
    {
        int num = playerNum - 1;
        Player player = PhotonNetwork.PlayerList[num];

        if(player.CustomProperties.ContainsKey(StringConstants.CustomProperties_PlayerHealth))
        {
            Debug.Log($"Updating UI for {playerNum}");
            //Debug.Log($"{player.CustomProperties[StringConstants.CustomProperties_PlayerHealth]}");
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) localPlayerHealthText.text = $"{player.CustomProperties[StringConstants.CustomProperties_PlayerHealth]}%";
        }
    }

    [PunRPC]
    public void ShowWinScreen(int playerNum)
    {

    }

    [PunRPC]
    public void ShowLoseScreen(int playerNum)
    {

    }
}