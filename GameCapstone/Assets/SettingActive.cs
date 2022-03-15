using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingActive : MonoBehaviour
{
    // Start is called before the first frame update

    public SoftPlatform sScript;

    void Start()
    {
        sScript = gameObject.GetComponent<SoftPlatform>();

        sScript.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
