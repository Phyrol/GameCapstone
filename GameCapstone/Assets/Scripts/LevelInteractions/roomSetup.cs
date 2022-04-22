using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class roomSetup : MonoBehaviourPunCallbacks
{
    public GameObject[] hazards;
    
    public GameObject[] softPlatforms;

    private void Start()
    {
        Debug.Log("--Room Set Up is called--");
        if (GameObject.Find("DeathWall") == null && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("--Making Death wall--");
            GameObject wall = PhotonNetwork.InstantiateRoomObject("DeathWALL", new Vector3(0, 0, 0), Quaternion.identity, 0, null);

            //if(wall != null)
            //    DontDestroyOnLoad(wall);
        }
        else
        {
            Debug.Log("There was already a Wall");
        }

        ////---------------Add hazard code -------------------//
        //if (hazards.Length == 0)
        //{
        //    hazards = GameObject.FindGameObjectsWithTag("Hazard");
        //}
        //foreach (GameObject obj in hazards)
        //{
        //    obj.AddComponent<Hazard>();
        //}

        ////---------------Add soft platform code -------------------//
        //if (softPlatforms.Length == 0)
        //{
        //    softPlatforms = GameObject.FindGameObjectsWithTag("Soft");
        //}
        //foreach (GameObject obj in softPlatforms)
        //{
        //    obj.AddComponent<SoftPlatform>();
        //}
    }


}
