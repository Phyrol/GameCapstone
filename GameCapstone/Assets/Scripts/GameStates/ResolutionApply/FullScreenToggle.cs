using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenToggle : MonoBehaviour
{
    public bool isFullScreen = false;
    public void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
    }
}
