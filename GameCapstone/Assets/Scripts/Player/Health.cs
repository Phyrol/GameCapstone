using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void Damage(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        Debug.Log($"DAMAGED: {direction}");
    }

    [PunRPC]
    void Dead()
    {
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("I AM DEAD");
    }
}
