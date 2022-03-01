using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMediumResolution : MonoBehaviour
{
    public Transform Toggle;
    void Update()
    {
        Screen.SetResolution(1280, 720, Toggle.transform.GetComponent<FullScreenToggle>().isFullScreen);
        Debug.Log("1280x720");
    }
}
