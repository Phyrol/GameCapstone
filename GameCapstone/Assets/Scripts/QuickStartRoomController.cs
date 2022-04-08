using Photon.Pun;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex = 1; // Number for the build index to the multiplay scene

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        base.OnDisable();
    }

    public override void OnJoinedRoom() //callback function for when we successfully create or join a room
    {
        Debug.Log("Joined room");
        StartGame();
        base.OnJoinedRoom();
    }

    private void StartGame() // Function for loading into the multiplayer scene
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex); // because of AutoSyncScene all players who join the room will load this scene
        }
    }
}
