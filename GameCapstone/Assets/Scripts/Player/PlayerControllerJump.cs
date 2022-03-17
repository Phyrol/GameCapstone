using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    NotMoving,
    Grounded,
    Jumping,
    Dodging,
    InAir,
};
public partial class PlayerController
{
    [System.Serializable]
    public class JumpVariables
    {
        [HideInInspector] public float scrollWheelDelta;
        public float jumpBuffer = .3f;
        public float jumpStrength = 6.5f;
        public float jumpStregthDecreaser = .05f;
        public float jumpInAirStrength = 0;
        public float jumpInAirControl = .1f;
        public float jumpingInitialGravity = -.3f;

        public float justJumpedCooldown = .1f;
        public float coyoteTime = 0.15f;

        public int inAirJumps = 1;
    }
    public void JumpInput()
    {
        jumpVariables.scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKeyDown(KeyCode.Space) || jumpVariables.scrollWheelDelta > 0)
        {
            _jumpBuffer = jumpVariables.jumpBuffer;
        }
    }
    public void HandleJumpInput()
    {
        if (_jumpBuffer <= 0) _jumpBuffer = 0;
        if(playerState != PlayerState.Dodging)
        {
            if (_jumpBuffer > 0 && (isGrounded || _coyoteTimer > 0) && playerState != PlayerState.Jumping) StartCoroutine(JumpCoroutine());
            else if (playerState == PlayerState.InAir && _inAirJumps > 0 && _jumpBuffer > 0)
            {
                _inAirJumps--;
                StartCoroutine(JumpCoroutine());
            }
        }
        
        if (_jumpBuffer > 0) _jumpBuffer -= Time.fixedDeltaTime;
    }
    private IEnumerator JumpCoroutine()
    {
        //SetAnimBool("IsJumping1", true);

        Debug.Log("JUMPING");
        //SetAnimTrigger("IsJumping2");

        SetVariablesOnJump();
        previousState = playerState;
        playerState = PlayerState.Jumping;
        y = jumpVariables.jumpStrength;
        g = jumpVariables.jumpingInitialGravity;
        totalVelocityToAdd += newRight;
        airControl = jumpVariables.jumpInAirControl;

        rb.velocity -= rb.velocity.y * Vector3.up;

        while (rb.velocity.y >= 0f && playerState != PlayerState.Grounded)
        {
            totalVelocityToAdd += Vector3.up * y;

            g = baseMovementVariables.initialGravity * jumpVariables.jumpStregthDecreaser;
            y -= jumpVariables.jumpStregthDecreaser;
            
            yield return fixedUpdate;
        }
        g = baseMovementVariables.initialGravity;

        if (playerState != PlayerState.Grounded)
        {
            g = 0;
            rb.velocity -= Vector3.up * rb.velocity.y;
            g = baseMovementVariables.initialGravity;
        }
        airControl = baseMovementVariables.inAirControl;
        //SetAnimBool("IsJumping1", false);

        previousState = playerState;
        if (!isGrounded) playerState = PlayerState.InAir;
    }
    public void SetVariablesOnJump()
    {
        _jumpBuffer = 0;
        _justJumpedCooldown = jumpVariables.justJumpedCooldown;
    }
}
