using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public PhotonView view;

    public ParticleSystem bloodspray;
    public float StartingPercent;
    public bool MovementDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
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
        if(gameObject.layer != 6)
        {
            GetComponentInChildren<Animator>().SetTrigger("isPunched");
            FindObjectOfType<AudioManager>().Play("Damage");

            bloodspray.Clear();
            bloodspray.Play();

            StartCoroutine(EmitTrail());

            GetComponent<Rigidbody>().AddForce(direction * (1 + StartingPercent / 100.0f), ForceMode.Impulse);
            Debug.Log($"DAMAGED: {direction * (1 + StartingPercent / 100.0f)}");
            StartingPercent += 5.0f;
            HealthDisplay.Instance.SetHealthDisplay(StartingPercent);
            StartCoroutine(KnockbackStun());
        }
    }

    [PunRPC]
    void Dead(int viewID)
    {
        if(view.ViewID == viewID)
        {
            //play death animation
            GetComponentInChildren<Animator>().SetTrigger("isDead");
            FindObjectOfType<AudioManager>().Play("Death");
            //PhotonNetwork.Destroy(gameObject);
            //temp:
            gameObject.GetComponent<PlayerController>().enabled = false;
            
            gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            gameObject.GetComponentInChildren<Melee>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // end temp
            Debug.Log("I AM DEAD");
        }
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
        GetComponent<CapsuleCollider>().material.bounciness = 1;
        GetComponent<PlayerController>().useGravity = false;
        yield return new WaitForSeconds(StartingPercent/60.0f);
        MovementDisabled = false;
        GetComponent<CapsuleCollider>().material.bounciness = 0;
        GetComponent<PlayerController>().useGravity = true;
    }
}
