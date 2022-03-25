using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IterativeEnabling : MonoBehaviour
{
    private SphereCollider col;
    private bool isRight = true;
    public float time = 0.5f;
    public float cooldownStartingValue = 0.6f;
    private float punchCooldown = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<SphereCollider>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && punchCooldown <= 0.0f)
        {
            Debug.Log("COLLISION ENABLED");
            FindObjectOfType<AudioManager>().Play("Punch");
            gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("isPunching");

            // play punch animation
            col.enabled = true;
            punchCooldown = cooldownStartingValue;
        }
        if(punchCooldown > 0.0f)
        {
            punchCooldown -= Time.deltaTime;
        }
        if(col.enabled)
        {
            time -= Time.deltaTime;
            if(time <= 0.0f)
            {
                time = 0.5f;
                col.enabled = false;
                Debug.Log("COLLISION DISABLED");
            }
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
        col.enabled = false;
    }
}
