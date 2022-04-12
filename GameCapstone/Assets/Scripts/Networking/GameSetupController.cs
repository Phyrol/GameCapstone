using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    public GameObject playerCam;
    public GameObject spawnPointList;
    private GameObject[] spawnPoints;

    private void Start()
    {
        spawnPoints = new GameObject[9];
        int i = 0;
        foreach(Transform child in spawnPointList.transform)
        {
            spawnPoints[i] = child.gameObject;
            i++;
        }

        //if(PhotonNetwork.IsMasterClient)
        //{
        //    GameObject wall = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "WALL"), Vector3.zero, Quaternion.identity);
        //    DontDestroyOnLoad(wall);
        //}

        CreatePlayer(); // create a networked player object for each player that loads into the mulplayer room
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");

        int rnd = Random.Range(0, spawnPoints.Length - 1);

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
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", $"PhotonPlayer{charName}"), spawnPoints[rnd].transform.position, Quaternion.identity);
        Destroy(spawnPoints[rnd]);

        DontDestroyOnLoad(player);
    }
}
