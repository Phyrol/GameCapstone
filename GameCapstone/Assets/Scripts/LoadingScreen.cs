using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void TriggerLoadScreen()
    {
        StartCoroutine(LoadLevelAnimation());
    }

    IEnumerator LoadLevelAnimation()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("Loading Screen");
    }
}
