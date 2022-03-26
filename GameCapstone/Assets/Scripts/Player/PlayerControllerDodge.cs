using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class DodgeVariables
    {
        public float dodgeBuffer = .3f;
        public float dodgeStrength = 13f;

        public float justDodgedCooldown = .1f;

        public int inAirDodges = 1;
    }

    public void DodgeInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _dodgeBuffer = dodgeVariables.dodgeBuffer;
        }
    }

    public void HandleDodgeInput()
    {
        if (_dodgeBuffer <= 0) _dodgeBuffer = 0;
        if(playerState != PlayerState.Jumping)
        {
            if (_dodgeBuffer > 0 && isGrounded && playerState != PlayerState.Dodging) StartCoroutine(DodgeCoroutine());
            else if (playerState == PlayerState.InAir && _inAirDodges > 0 && _dodgeBuffer > 0)
            {
                _inAirDodges--;
                StartCoroutine(DodgeCoroutine());
            }
        }
        if (_dodgeBuffer > 0) _dodgeBuffer -= Time.fixedDeltaTime;
    }

    private IEnumerator DodgeCoroutine()
    {
        Debug.Log("DODGING");

        //SetAnimBool("isDodging1", true);
        FindObjectOfType<AudioManager>().Play("Dodge");

        // trail on
        GetComponentInChildren<TrailRenderer>().emitting = true;
        SetAnimTrigger("isDodging2");

        SetVariablesOnDodge();
        previousState = playerState;
        playerState = PlayerState.Dodging;

        rb.velocity = x == 0 ? transform.right * dodgeVariables.dodgeStrength : currentRight.normalized * dodgeVariables.dodgeStrength;

        // when animation ends, give control back to player
        yield return new WaitForSeconds(1f);

        // trail off
        GetComponentInChildren<TrailRenderer>().emitting = false;
        currentRight.x = 0;

        previousState = playerState;
        if (!isGrounded) playerState = PlayerState.InAir;
        else playerState = PlayerState.Grounded;
        
    }
    public void SetVariablesOnDodge()
    {
        _dodgeBuffer = 0;
        _justDodgedCooldown = dodgeVariables.justDodgedCooldown;
    }
}
