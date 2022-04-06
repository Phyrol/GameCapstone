using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private string playerPrefab;

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
}
