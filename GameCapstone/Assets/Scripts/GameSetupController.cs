using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    public GameObject playerCam;

    private void Start()
    {
        CreatePlayer(); // create a networked player object for each player that loads into the mulplayer room
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");

        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), new Vector3(0, 2, 0), Quaternion.identity);

        DontDestroyOnLoad(player);
    }
}
