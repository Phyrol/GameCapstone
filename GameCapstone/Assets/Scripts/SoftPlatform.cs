using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))] //for now, not necessary
public class SoftPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;
    [SerializeField] private Vector3 triggerScale = Vector3.one * 4.0f;
    [SerializeField] bool below;
    [SerializeField] bool down;

    [SerializeField] double timeRemaining = 1;
    [SerializeField] bool timerIsRunning = false;

    private GameObject player;
    PlayerController pScript;

    private new BoxCollider collider = null;
    private BoxCollider collisionCheck = null;

    public Vector3 Direction => localDirection ? 
        transform.TransformDirection(entryDirection.normalized) : entryDirection.normalized;
    
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        //collider.enabled = true;
        collider.isTrigger = false;

        collisionCheck = gameObject.AddComponent<BoxCollider>();

        
        collisionCheck.size = new Vector3(
            collider.size.x,
            collider.size.y * triggerScale.y,
            collider.size.z
        );
        collisionCheck.center = collider.center;
        collisionCheck.isTrigger = true;

        below = true;
        down = false;

        //player = GameObject.Find("Player");
        //pScript = player.GetComponent<PlayerController>();
    }

    /*
    private void FixedUpdate()
    {
        gameObject.SetActive(true);

        if (Input.GetKey(KeyCode.S))
        {
            down = true;
            timerIsRunning = true;

            Debug.Log("pressing down s ");
        }
        else if(!timerIsRunning)
        {
            down = false;
            collisionCheck.enabled = true;
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                if(gameObject.transform.position.y > player.transform.position.y)
                {
                    timeRemaining = 1;
                    timerIsRunning = false;
                }
            }
            else
            {
                timeRemaining = 1;
                timerIsRunning = false;
                collisionCheck.enabled = true;
            }
        }


    }*/


    private void OnTriggerStay(Collider other)
    {


   
        Physics.IgnoreCollision(collider, other, true);

        below = Physics.ComputePenetration(
            collisionCheck, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float _);
        below = true;
        if (below)
        {
            float dot = Vector3.Dot(Direction, collisionDirection);
            Debug.Log(Direction + " " + collisionDirection);
            //this vvv makes opposite direction passing not aloowed - which we will have to change 
            if (dot < 0)
            {
                if(!down)
                { 
                Physics.IgnoreCollision(collider, other, false);
                Debug.Log("In Stay (false condition) and we keep collision" + dot);
                }
                else
                {
                    Physics.IgnoreCollision(collider, other, true);
                    collisionCheck.enabled = false;
                    Debug.Log("pressing down on top");
                }
            }
            else
            {
                Physics.IgnoreCollision(collider, other, true);
                Debug.Log("In Stay (true condition) and we ignore collision" + dot);
            }
        }
        else// if above
        {
            float dot = Vector3.Dot(Direction, collisionDirection);
            //this vvv makes opposite direction passing not aloowed - which we will have to change 
            if (dot > 0)
            {
                Physics.IgnoreCollision(collider, other, false);
                Debug.Log("In S (false condition) and we keep collision");
                //below = false;
            }
            else
            {
                Physics.IgnoreCollision(collider, other, true);
                Debug.Log("In Stay (true condition) and we ignore collision");
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit collider");
    }

    private void OnValidate()
    {
        // This bit of code exists only to prevent OnDrawGizmos from throwing
        // errors in the editor mode when it does not have reference to the
        // collider, if used.
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 direction;
        if (localDirection)
        {
            
            direction = transform.TransformDirection(entryDirection.normalized);
        }
        else
        {
            direction = entryDirection;
        }
            
        
        //this is where you are blocked 
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, entryDirection);

        //this is where you can enter from
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -entryDirection);
    }
}
