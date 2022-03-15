using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class SoftPlatformVariables
    {
        public double timeRemaining = 1;
        public bool timerIsRunning = false;
        public bool down;
    }

    public void DownPlatform()
    {
        //gameObject.SetActive(true);

        //if()

        if (Input.GetKey(KeyCode.S))
        {
            platformVariables.down = true;
            platformVariables.timerIsRunning = true;
        }
        else if (!platformVariables.timerIsRunning)
        {
            platformVariables.down = false;
            //platformVariables.collisionCheck.enabled = true;
        }

        if (platformVariables.timerIsRunning)
        {
            if (platformVariables.timeRemaining > 0)
            {
                platformVariables.timeRemaining -= Time.deltaTime;

                if (transform.position.y > transform.position.y)//wrong
                {
                    platformVariables.timeRemaining = 1;
                    platformVariables.timerIsRunning = false;
                }
            }
            else
            {
                platformVariables.timeRemaining = 1;
                platformVariables.timerIsRunning = false;
                //platformVariables.collisionCheck.enabled = true;
            }
        }


    }
}
