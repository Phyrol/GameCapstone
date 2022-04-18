using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    public GameObject playerCam;
    private GameObject spawnPointList;
    private GameObject[] spawnPoints = new GameObject[9];
    public int numberPlayers;

    private PhotonView view;
    private int spawnNum;

    private void Start()
    {
        view = gameObject.GetComponent<PhotonView>();

        spawnPointList = GameObject.Find("SpawnPoints");

        //spawnPoints = new GameObject[9];
        int i = 0;
        foreach (Transform child in spawnPointList.transform)
        {
            spawnPoints[i] = child.gameObject;
            i++;
        }
            
        CreatePlayer();

         // create a networked player object for each player that loads into the mulplayer room
    }

    private void CreatePlayer()
    {      
        Debug.Log("Creating Player");

        string charName;
        GameObject playerMng = GameObject.Find("PlayerManager");
        switch(playerMng?.GetComponent<PlayerManager>().GetPlayerCharacter())
        {
            case "pumpkinheadRIGGED":
                charName = "Pumpkin";
                break;
            case "vampRIGGED":
                charName = "Vampire";
                break;
            case "werewolfRIGGED":
            charName = "Werewolf";
            break;
            default:
                charName = "Pumpkin";
                break;
        }
        Debug.Log($"Spawning: {charName}");
        //GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", $"PhotonPlayer{charName}"), spawnPos, Quaternion.identity);
        spawnNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if(spawnNum > 9)
        {
            Debug.Log("Hitting limit!!!");
            spawnNum = 0;
        }
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", $"PhotonPlayer{charName}"), spawnPoints[spawnNum].transform.position, Quaternion.identity);
        //Destroy(spawnPoints[rnd]);

        DontDestroyOnLoad(player);
    }
}

