using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMinResolution : MonoBehaviour
{
    public Transform Toggle;
    void Update()
    {
        Screen.SetResolution(854, 480, Toggle.transform.GetComponent<FullScreenToggle>().isFullScreen);
        Debug.Log("854x480");
    }
}
