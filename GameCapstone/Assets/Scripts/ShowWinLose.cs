using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowWinLose : MonoBehaviour
{
    public GameObject gameWinOverlay;
    public GameObject gameLoseOverlay;

    public void showWinScreen()
    {
        gameWinOverlay.SetActive(true);
        Cursor.visible = true;
    }

    public void showLoseScreen()
    {
        gameLoseOverlay.SetActive(true);
        Cursor.visible = true;
    }
}
