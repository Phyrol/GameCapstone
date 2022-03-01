using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaxResolution : MonoBehaviour
{
    public Transform Toggle;
    void Update()
    {
        Screen.SetResolution(1920, 1080, Toggle.transform.GetComponent<FullScreenToggle>().isFullScreen);
        Debug.Log("1920x1080");
    }
}
