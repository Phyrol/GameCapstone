using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class roomSetup : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("--Room Set Up is called--");
        if (GameObject.Find("DeathWall") == null && PhotonNetwork.IsMasterClient) {
            Debug.Log("--Making Death wall--");
            //PhotonNetwork.Instantiate("DeathWall", new Vector3(0, 0, 0), Quaternion.identity, 0);
            PhotonNetwork.InstantiateRoomObject("DeathWall", new Vector3(0, 0, 0), Quaternion.identity, 0);
        }
        else
        {
            Debug.Log("There was already a Wall");
        }
    }
    
}
