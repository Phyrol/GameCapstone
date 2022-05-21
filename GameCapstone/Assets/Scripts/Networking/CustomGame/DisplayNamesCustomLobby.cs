using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayNamesCustomLobby : MonoBehaviour
{
    [SerializeField]
    private GameObject nameSlots;

    [SerializeField]
    private GameObject usernameText;

    public void AddPlayer(string name, int slot)
    {
        //nameSlots.GetComponentsInChildren<TextMeshProUGUI>()[slot].text = name;
        Instantiate(usernameText, nameSlots.transform);
    }
}
