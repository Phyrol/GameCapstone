using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UsernameCheck : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject InputScreen;
    
    //public GameObject ErrorText;
    private string username { set; get; }

    private void Start()
    {
        if(!PlayerManager.instance.GetPlayerName().Equals(""))
        {
            Debug.Log($"Player name is: {PlayerManager.instance.GetPlayerName()}");
            MainMenu.SetActive(true);
            InputScreen.SetActive(false);
        }
    }

    public void NameCheck()
    {
        GameObject errorText = InputScreen.transform.GetChild(0).gameObject;
        Text[] newText = GetComponentsInChildren<Text>();
        string found = newText[1].text;
        bool bad = false;
                
        //Debug.Log("checking username: " + found);

        char[] charsToTrim = { '*', ' ', '\'', '$'};
        string result = found.Trim(charsToTrim);

        bad = badWords(result);

        if (result.Equals("") || result.Length == 0 || bad == true)
        {
            errorText.GetComponent<TextMeshProUGUI>().text = "Name not accepted";
            //ErrorText.GetComponent<TextMeshProUGUI>().text = "Name not accepted";
        }
        else
        {
            //Debug.Log("username accepted");
            username = result;
            SubmitName(username);
            MainMenu.SetActive(true);
            InputScreen.SetActive(false);
        }
    }

    public void SubmitName(string name)
    {
        PlayerManager.instance.GetComponent<PlayerManager>().SetPlayerName(name);
    }

    private bool badWords(string result)
    {
        string[] badwords = {"fuck", "jerk", "fuc", "shit"};
        foreach (string word in badwords)
        {
            if (result.Contains(word))
            {
                //Debug.Log("bad word found");
                return true;
            }
        }
        return false;
    }
}
