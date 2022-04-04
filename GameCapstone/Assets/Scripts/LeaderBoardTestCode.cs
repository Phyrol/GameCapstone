using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardTestCode : MonoBehaviour
{
    public TextMeshProUGUI Username, MatchCount, WinCount, WinPercentage;
    int maxScores;
    int currentScores = 0;
    public TextMeshProUGUI[] Entries;

    void Start()
    {
        maxScores = Entries.Length;
    }

    public void SubmitScore()
    {
        if (currentScores < maxScores)
        {
            // replace input field values with database values
            Entries[currentScores].SetText( 
                (currentScores + 1).ToString() 
                + "   		" 
                + Username.text 
                + "​    		          " 
                + MatchCount.text 
                + "   			      " 
                + WinCount.text 
                + "​    		   " 
                + WinPercentage.text);
            currentScores++;
        }
    }
}
