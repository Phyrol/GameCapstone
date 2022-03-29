using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public ParticleSystem bloodspray;
    public float StartingPercent;
    public bool MovementDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude >= 0 - Mathf.Epsilon && GetComponent<Rigidbody>().velocity.magnitude <= 0 + Mathf.Epsilon && MovementDisabled)
        {
            MovementDisabled = false;
        }
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
        StartCoroutine(KnockbackStun());
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

    IEnumerator KnockbackStun()
    {
        MovementDisabled = true;
        yield return new WaitForSeconds(StartingPercent/60.0f);
        MovementDisabled = false;
    }
}
