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
    private Vector3 spawnPos;

    private void Start()
    {
        view = gameObject.GetComponent<PhotonView>();

        spawnPointList = GameObject.Find("SpawnPoints");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master sets the num of players");
            numberPlayers = 0;
            view.RPC("SetAll", RpcTarget.All, 0);
        }
        else
        {
            Debug.Log("not master");
        }

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
        //Debug.Log("before spawn: " + numberPlayers);

        numberPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        spawnPos = Spawn(); 

        //Debug.Log("after spawn: " + numberPlayers);


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
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", $"PhotonPlayer{charName}"), spawnPos, Quaternion.identity);
        //Destroy(spawnPoints[rnd]);

        DontDestroyOnLoad(player);
    }

    private Vector3 Spawn()
    {

        if (numberPlayers < 10)
        {
            //Debug.Log("make " + numberPlayers +" player");
            spawnPos = spawnPoints[numberPlayers - 1].transform.position;
        }
        else
        {
            spawnPos = spawnPoints[0].transform.position;
        }      
        return spawnPos;
    }
}
