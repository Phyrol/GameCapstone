using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] //for now, not necessary
public class SoftPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;

    private new BoxCollider collider = null;

    public Vector3 Direction => localDirection ? 
        transform.TransformDirection(entryDirection.normalized) : entryDirection.normalized;
    
    private void Awake()
    {
        collider = transform.parent.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collider, other, true);
            other.GetComponent<PlayerController>().SetActiveMask(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collider, other, false);
            other.GetComponent<PlayerController>().SetActiveMask(true);
        }
    }
}
