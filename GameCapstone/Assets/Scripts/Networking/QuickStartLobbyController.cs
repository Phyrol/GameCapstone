using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject quickStartButton; // button used for creating and joining a game
    [SerializeField]
    private GameObject quickCancelButton; // button used to stop searching for a game to join
    [SerializeField]
    private int RoomSize; // manual set the number of players in the room at one time

    [SerializeField]
    private TextMeshProUGUI countBack;
    [SerializeField]
    private TextMeshProUGUI countFront;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // makes it so whatever scene the master client has the players have
        quickStartButton.GetComponent<Button>().enabled = true;
        base.OnConnectedToMaster();
    }

    public void QuickStart() // paired to quickstart button
    {
        quickStartButton.GetComponent<Button>().enabled = false;
        StartCoroutine(startCountdown());
        //quickCancelButton.SetActive(true);
        //PhotonNetwork.JoinRandomRoom(); // first tries to join an existing room
        Debug.Log("Quick start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
        base.OnJoinRandomFailed(returnCode, message);
    }

    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps); // attempt to create a new room
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying again.");
        CreateRoom(); // retry to create room with different name
        base.OnCreateRoomFailed(returnCode, message);
    }

    public void QuickCancel()
    {
        //quickCancelButton.SetActive(false);
        quickStartButton.GetComponent<Button>().enabled = true;
        PhotonNetwork.LeaveRoom();
    }

    private IEnumerator startCountdown()
    {
        Debug.Log("counting down");
        for( int i = 5; i > 0; i-- )
        {
            countBack.text = i.ToString();
            countFront.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        FindObjectOfType<LoadingScreen>().TriggerLoadScreen();
        PhotonNetwork.JoinRandomRoom();
    }
}
