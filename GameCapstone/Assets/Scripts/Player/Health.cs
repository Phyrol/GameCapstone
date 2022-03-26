using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public ParticleSystem bloodspray;
    public float StartingPercent = 0.0f;

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

        StartCoroutine(EmitTrail());

        GetComponent<Rigidbody>().AddForce(direction * (1 + StartingPercent/100.0f), ForceMode.Impulse);
        Debug.Log($"DAMAGED: {direction * (1 + StartingPercent / 100.0f)}");
        StartingPercent += 5.0f;
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

    IEnumerator EmitTrail()
    {
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        trail.emitting = true;
        yield return new WaitForSeconds(2f);
        trail.emitting = false;
    }
}
