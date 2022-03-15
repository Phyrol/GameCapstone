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

        if(PhotonNetwork.IsMasterClient)
        {
            GameObject wall = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "WALL"), Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(wall);
        }

        CreatePlayer(); // create a networked player object for each player that loads into the mulplayer room
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");

        int rnd = Random.Range(0, spawnPoints.Length - 1);

        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), spawnPoints[rnd].transform.position, Quaternion.identity);
        Destroy(spawnPoints[rnd]);

        DontDestroyOnLoad(player);
    }
}
