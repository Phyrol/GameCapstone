using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IterativeEnabling : MonoBehaviour
{
    private SphereCollider col;
    private bool isRight = true;
    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("COLLISION ENABLED");
            FindObjectOfType<AudioManager>().Play("Punch");
            gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("isPunching");

            // play punch animation
            col.enabled = !col.enabled;
        }
        if (Input.GetKey(KeyCode.D))
            isRight = true;
        else if (Input.GetKey(KeyCode.A))
            isRight = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("DAMAGING");
            other.gameObject.GetComponent<PhotonView>().RPC("Damage", RpcTarget.Others, new object[] { new Vector3(transform.parent.transform.right.x * 10, 5, 0) });
        }
        //other.GetComponent<Collider>().attachedRigidbody.AddForce(transform.parent.transform.right.x * 10, 5, 0, ForceMode.Impulse);

        //if(isRight)
        //    other.GetComponent<Collider>().attachedRigidbody.AddForce(10, 5, 0, ForceMode.Impulse);
        //else
        //    other.GetComponent<Collider>().attachedRigidbody.AddForce(-10, 5, 0, ForceMode.Impulse);
        //col.enabled = false;
    }
}
