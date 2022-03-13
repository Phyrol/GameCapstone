using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSelection : MonoBehaviour
{
    private GameObject[] resList;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        resList = new GameObject[transform.childCount];

        for( int i = 0; i < transform.childCount; i++ )
        {
            resList[i] = transform.GetChild(i).gameObject;
        }

        foreach( GameObject go in resList )
        {
            go.SetActive(false);
        }

        if ( resList[0] ) resList[0].SetActive(true);


    }

    public void ToggleLeft()
    {
        // hide current character model
        resList[index].SetActive(false);

        index--;

        if( index < 0 ) index = resList.Length - 1;

        // show next character model
        resList[index].SetActive(true);
    }

    public void ToggleRight()
    {
        // hide current character model
        resList[index].SetActive(false);

        index++;

        if( index == resList.Length ) index = 0;

        // show next character model
        resList[index].SetActive(true);
    }
}
