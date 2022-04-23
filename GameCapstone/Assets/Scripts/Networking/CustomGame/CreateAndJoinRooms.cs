using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public Button hostStartButton;
    public TextMeshProUGUI countFront;
    public TextMeshProUGUI countBack;

    [SerializeField]
    private GameObject failedToJoinText;

    private byte maxPlayers = 8;

    void Start()
    {
        // The start button will only appear for the host
        if (PhotonNetwork.IsMasterClient && hostStartButton != null ) hostStartButton.interactable = true;
        else if ( hostStartButton != null) hostStartButton.gameObject.SetActive(false);
    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;

        PhotonNetwork.CreateRoom(createInput.text, options);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        failedToJoinText.GetComponent<Text>().text = $"Failed to Join Room: {message}";
        failedToJoinText.SetActive(true);
    }

    public void StartCustomGame()
    {
        if (PhotonNetwork.IsMasterClient) StartCoroutine(startCountdown());
    }

    public void LoadMainLevel()
    {
        Debug.Log("Starting Game");
        PhotonNetwork.LoadLevel(1); // because of AutoSyncScene all players who join the room will load this scene
    }

    public IEnumerator startCountdown()
    {
        Debug.Log("counting down . . .");
        for( int i = 5; i > 0; i-- )
        {
            countFront.text = i.ToString();
            countBack.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        FindObjectOfType<LoadingScreen>().TriggerLoadScreen();
        yield return new WaitForSeconds(1f);

        LoadMainLevel();
    }
}
