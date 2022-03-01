using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    float interpolateCount = 0.0f;
    float start = 0.0f;
    float end = 360.0f;
    public float interpolationRate = 0.001f;
    
    // Uses the interpolation rate and Lerp to rotate the skybox material at a fixed rate.
    // The rate was made public to allow for easy editing.
    void FixedUpdate()
    {
        //RenderSettings.skybox.SetFloat("_Rotation", Mathf.Lerp(start, end, interpolateCount));
        //interpolateCount += interpolationRate;
        //if(RenderSettings.skybox.GetFloat("_Rotation") >= 360)
        //{
        //    RenderSettings.skybox.SetFloat("_Rotation", 0);
        //    interpolateCount = 0.0f;
        //}
    }
}
