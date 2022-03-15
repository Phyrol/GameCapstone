using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            col.enabled = !col.enabled;
        }
        if (Input.GetKey(KeyCode.D))
            isRight = true;
        else if (Input.GetKey(KeyCode.A))
            isRight = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isRight)
            other.GetComponent<Collider>().attachedRigidbody.AddForce(10, 5, 0, ForceMode.Impulse);
        else
            other.GetComponent<Collider>().attachedRigidbody.AddForce(-10, 5, 0, ForceMode.Impulse);
        col.enabled = false;
    }
}
