using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public ParticleSystem bloodspray;

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
        GetComponentInChildren<Animator>().SetTrigger("isPunched");
        FindObjectOfType<AudioManager>().Play("Damage");
        bloodspray.Clear();
        bloodspray.Play();

        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        Debug.Log($"DAMAGED: {direction}");
    }

    [PunRPC]
    void Dead()
    {
        //play death animation
        GetComponentInChildren<Animator>().SetTrigger("isDead");
        FindObjectOfType<AudioManager>().Play("Death");
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("I AM DEAD");
    }
}
