using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public Button hostStartButton;
    public TextMeshProUGUI countFront;
    public TextMeshProUGUI countBack;

    void Start()
    {
        // The start button will only appear for the host
        if (PhotonNetwork.IsMasterClient) hostStartButton.interactable = true;
        else hostStartButton.gameObject.SetActive(false);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
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
