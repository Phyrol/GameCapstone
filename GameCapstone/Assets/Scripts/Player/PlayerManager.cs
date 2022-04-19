using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private string playerPrefab;

    [SerializeField]
    private string playerName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerCharacter(string character)
    {
        playerPrefab = character;
    }

    public string GetPlayerCharacter()
    {
        return playerPrefab;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}
