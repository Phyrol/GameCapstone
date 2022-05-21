using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour //PunCallbacks
{
    public GameObject playerCam;
    private GameObject spawnPointList;
    private List<GameObject> spawnPoints;
    public int numberPlayers;

    private PhotonView view;
    private int spawnNum;
    private int scramble;
    private bool once;

    private void Start()
    {
        spawnPoints = new List<GameObject>();
        view = gameObject.GetComponent<PhotonView>();

        spawnPointList = GameObject.Find("SpawnPoints");

        //spawnPoints = new GameObject[9];
        int i = 0;
        foreach (Transform child in spawnPointList.transform)
        {
            spawnPoints.Add(child.gameObject);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("I am master");
            scramble = (int)(Random.Range(0, spawnPoints.Count));
            view.RPC("SetAll", RpcTarget.AllBuffered, scramble);
        }

        once = true;
        CreatePlayer();

         // create a networked player object for each player that loads into the mulplayer room
    }

    [PunRPC]
    void SetAll(int number)
    {
        //Debug.Log("scramble set to" + number);
        scramble = number;
    }

    private void Update()
    {
        //if (once)
        //{
        //    CreatePlayer();
        //    once = false;
        //}
           
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
        //Debug.Log("Player " + spawnNum + " and scramble = " + scramble);
        spawnNum += scramble;
        if(spawnNum > 8)
        {
            //Debug.Log("Hitting limit!");
            spawnNum -= 9;
        }
        
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", $"PhotonPlayer{charName}"), spawnPoints[spawnNum].transform.position, Quaternion.identity);
        gameObject.GetComponent<PhotonView>().RPC("RemoveSpawnPoint", RpcTarget.All, new object[] { spawnNum });
        //Destroy(spawnPoints[rnd]);

        //DontDestroyOnLoad(player);
    }

    [PunRPC]
    void RemoveSpawnPoint(int num)
    {
        spawnPoints.RemoveAt(num);
    }
}

