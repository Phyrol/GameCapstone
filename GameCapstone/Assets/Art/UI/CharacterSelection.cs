using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        characterList = new GameObject[transform.childCount];

        for( int i = 0; i < transform.childCount; i++ )
        {
            characterList[i] = transform.GetChild(i).gameObject;
        }

        foreach( GameObject go in characterList )
        {
            go.SetActive(false);
        }

        if ( characterList[0] ) characterList[0].SetActive(true);


    }

    public void ToggleLeft()
    {
        // hide current character model
        characterList[index].SetActive(false);

        index--;

        if( index < 0 ) index = characterList.Length - 1;

        // show next character model
        characterList[index].SetActive(true);
    }

        public void ToggleRight()
    {
        // hide current character model
        characterList[index].SetActive(false);

        index++;

        if( index == characterList.Length ) index = 0;

        // show next character model
        characterList[index].SetActive(true);
    }
}
