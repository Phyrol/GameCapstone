using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Melee : MonoBehaviour
{
    PhotonView view;
    private SphereCollider col;
    [SerializeField]
    private float punchDamage = 5.0f;
    private bool isRight = true;
    public float time = 0.5f;
    public float cooldownStartingValue = 0.6f;
    private float punchCooldown = 0.0f;
    public Health healthComponent;
    // Start is called before the first frame update
    void Start()
    {
        view = transform.parent.GetComponent<PhotonView>();
        col = gameObject.GetComponent<SphereCollider>();
        col.enabled = false;
        healthComponent = gameObject.transform.parent.gameObject.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && punchCooldown <= 0.0f && !healthComponent.MovementDisabled)
        {
            if(view.IsMine)
            {
                Debug.Log("COLLISION ENABLED");
                FindObjectOfType<AudioManager>().Play("Punch");
                gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("isPunching");

                // play punch animation
                col.enabled = true;
                punchCooldown = cooldownStartingValue;
            }
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
            //Debug.Log("DAMAGING");
            other.gameObject.GetComponent<PhotonView>().RPC("MeleeDamage", RpcTarget.Others, new object[] { new Vector3(transform.parent.transform.right.x * 10, 5, 0), punchDamage });
        }
        if(col != null) col.enabled = false;
    }
}
