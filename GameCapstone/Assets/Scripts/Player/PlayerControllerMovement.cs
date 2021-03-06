using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public Animator animator;
    public GameObject mesh;
    public ParticleSystem dirtparticles;

    bool rotatedLeft = false;
    bool rotatedRight = true;
    [System.Serializable]
    public class BaseMovementVariables
    {
        #region Variables

        #region General
        [Header("General")]
        public float maxSlope = 60;
        public float groundCheckDistance;
        #endregion

        #region Acceleration
        [Header("Acceleration")]
        public float walkSpeedIncrease = 1;
        public float sprintSpeedIncrease = 2;
        #endregion

        #region Velocity Caps
        [Header("Velocity Boundaries")]
        public float maxWalkVelocity = 20f;
        public float maxSprintVelocity = 15;
        public float maxInAirVelocity = 15f;
        public float minVelocity = .1f;
        #endregion

        #region Friction
        [Header("Friction Values")]
        public float groundFriction = .1f;
        public float inAirFriction = .004f;
        #endregion

        #region In Air
        [Header("In Air Variables")]
        [Range(0, 1)]
        public float inAirControl = .021f;
        #endregion

        #region Gravity
        [Header("Gravity Variables")]
        public float initialGravity = -.55f;
        public float gravityRate = 1.008f;
        public float maxGravity = -39.2f;
        #endregion

        #region Fake Ground Checks
        [Header("Fake Ground Variables")]
        public float fakeGroundTime = .1f;
        [HideInInspector] public float _fakeGroundTimer;
        [HideInInspector] public bool feetSphereCheck;
        [HideInInspector] public bool kneesCheck;
        #endregion

        #endregion

        public void StartVariables(CapsuleCollider capCollider) => groundCheckDistance = capCollider.height * .5f - capCollider.radius;
    }

    private void MovementInput()
    {

        speedIncrease = baseMovementVariables.walkSpeedIncrease;
        maxVelocity = baseMovementVariables.maxWalkVelocity;

        if (Input.GetKey(KeyCode.D))
        {
            x = speedIncrease;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            x = speedIncrease;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        //else if (Input.GetAxis("GamePadHorizontal") != 0) x = Input.GetAxis("GamePadHorizontal") * speedIncrease;
        else x = 0;
        
        if (Input.GetKey(KeyCode.S))
        {
            if (Physics.SphereCast(transform.position, capCollider.radius, -transform.up, out hit, baseMovementVariables.groundCheckDistance + 0.01f, softPlatform) && hit.transform.CompareTag("SoftPlatform"))
            {
                //Debug.Log($"on: {hit.transform.name}");
                Physics.IgnoreCollision(capCollider, hit.transform.GetComponent<BoxCollider>(), true);
                SetActiveMask(false);
            }
        }
        
        if (Mathf.Abs(x) > 0) SetAnimFloat("Speed", 1);
        else SetAnimFloat("Speed", 0);
    }
    private void GroundCheck()
    {
        if (jumpMechanic)
        {
            if (_coyoteTimer > 0) _coyoteTimer -= Time.fixedDeltaTime;
            if (jumpVariables.justJumpedCooldown > 0) _justJumpedCooldown -= Time.fixedDeltaTime;
        }
        if(dodgeMechanic)
        {
            //if (_justDodgedCooldown <= 0) Debug.Log("can dodge");
            if (dodgeVariables.justDodgedCooldown > 0) _justDodgedCooldown -= Time.fixedDeltaTime;
        }
        groundCheck = (!jumpMechanic || _justJumpedCooldown <= 0 || _justDodgedCooldown <= 0) ? Physics.SphereCast(transform.position, capCollider.radius, -transform.up, out hit, baseMovementVariables.groundCheckDistance + 0.01f, ~_activeMask) : false;
        surfaceSlope = Vector3.Angle(hit.normal, Vector3.up);
        if (surfaceSlope > baseMovementVariables.maxSlope)
        {
            groundCheck = false;
            if (playerState != PlayerState.Jumping && playerState != PlayerState.InAir)
            {
                previousState = playerState;
                playerState = PlayerState.InAir;
                g = baseMovementVariables.initialGravity;
            }
        }
        totalVelocityToAdd = Vector3.zero;
        newRight = Vector3.zero;

        groundedForward = Vector3.Cross(hit.normal, -transform.right);
        groundedRight = Vector3.Cross(hit.normal, transform.forward);

        //Change the value of the groundcheck if the player is on the fakeGround state
        if (onFakeGround)
        {
            if (groundCheck) onFakeGround = false;
            else
            {
                groundCheck = true;
                groundedForward = transform.forward;
                groundedRight = transform.right;
            }
        }
        //Player just landed
        if (groundCheck && (playerState == PlayerState.Jumping || playerState == PlayerState.InAir))
        {
            rb.velocity = rb.velocity - Vector3.up * rb.velocity.y;
            float angleOfSurfaceAndVelocity = Vector3.Angle(rb.velocity, (hit.normal - Vector3.up * hit.normal.y));
            if (!onFakeGround && hit.normal.y != 1 && angleOfSurfaceAndVelocity < 5)
                rb.velocity = (groundedRight * x + groundedForward).normalized * rb.velocity.magnitude;          //This is to prevent the weird glitch where the player bounces on slopes if they land on them without jumping
            friction = baseMovementVariables.groundFriction;
            _inAirJumps = jumpVariables.inAirJumps;
            _inAirDodges = dodgeVariables.inAirDodges;
            previousState = playerState;
            playerState = PlayerState.Grounded;

            dirtparticles.Clear();
            dirtparticles.Play();

            if (playerJustLanded != null) playerJustLanded();

            g = 0;
        }
        //Player just left the ground
        if (isGrounded && !groundCheck)
        {
            if (playerState != PlayerState.Jumping)
            {
                previousState = playerState;
                playerState = PlayerState.InAir;
                SetInitialGravity();
            }
            friction = baseMovementVariables.inAirFriction;
            _coyoteTimer = jumpVariables.coyoteTime;
            if (playerLeftGround != null) playerLeftGround();
        }
        isGrounded = groundCheck;

        //If close to a small step, raise the player to the height of the step for a smoother feeling movement
        float maxDistance = capCollider.radius;
        if (playerState == PlayerState.Grounded) baseMovementVariables.feetSphereCheck = Physics.SphereCast(transform.position - Vector3.up * .5f, capCollider.radius + .01f, rb.velocity.normalized, out feetHit, maxDistance, ~triggers);
        if (baseMovementVariables.feetSphereCheck && !onFakeGround)
        {
            Vector3 direction = feetHit.point - (transform.position - Vector3.up * .5f);
            float dist = direction.magnitude;
            baseMovementVariables.kneesCheck = Physics.Raycast(transform.position - Vector3.up * capCollider.height * .24f, (direction - rb.velocity.y * Vector3.up), dist, ~triggers);
            if (!baseMovementVariables.kneesCheck && playerState == PlayerState.Grounded && x != 0)
            {
                //StartCoroutine(FakeGround());
                isGrounded = true;
            }
        }
        else baseMovementVariables.kneesCheck = false;
    }
    private void Move()
    {
        currentRight = new Vector3(rb.velocity.x, 0, 0);

        if (!isGrounded)
        {
            newRight = transform.right.normalized * x;
            if (x != 0)
            {
                if (Mathf.Abs(rb.velocity.x) < baseMovementVariables.maxInAirVelocity)
                {
                    totalVelocityToAdd.x += newRight.x;
                }
                else
                {
                    rb.velocity = new Vector3((transform.right.normalized * baseMovementVariables.maxInAirVelocity).x, rb.velocity.y, rb.velocity.z);
                }

                //rb.velocity = newRight.normalized * currentRight.magnitude * airControl + currentRight * (1f - airControl) + rb.velocity.y * Vector3.up;
            }
        }
        else
        {
            newRight = groundedRight.normalized * x;
            if (hit.normal.y == 1)
            {
                newRight = new Vector3(newRight.x, 0, 0);
                rb.velocity = (rb.velocity - Vector3.up * rb.velocity.y).normalized * rb.velocity.magnitude;
            }

            if (rb.velocity.magnitude < maxVelocity)
            {
                totalVelocityToAdd += newRight;
            }
            else
            {
                if (x == 0 || (pvX < 0 && x > 0)
                    || (x < 0 && pvX > 0)) rb.velocity *= .99f; //If the palyer changes direction when going at the maxSpeed then decrease speed for smoother momentum shift
                else if (rb.velocity.magnitude < maxVelocity + 1f) rb.velocity = newRight.normalized * maxVelocity;
                totalVelocityToAdd = Vector3.zero;
            }

            if (x == 0)
            {
                currentRight.x = 0;
                rb.velocity = currentRight;
            }
            else if (rb.velocity.magnitude != maxVelocity)
            {
                totalVelocityToAdd -= rb.velocity * friction;
            }

            pvX = x;
        }
    }
    public void SetInitialGravity() => g = baseMovementVariables.initialGravity;
    private void ApplyGravity()
    {
        if (useGravity)
        {
            if (!isGrounded)
            {
                totalVelocityToAdd += Vector3.up * g;
            }
            if (g > baseMovementVariables.maxGravity) g *= baseMovementVariables.gravityRate;
        }
    }
    public void ToggleGravity(bool active)
    {
        previousState = playerState;
        playerState = PlayerState.InAir;
        if (active)
        {
            useGravity = true;
            SetInitialGravity();
        }
        else
        {
            useGravity = false;
            g = 0;
        }
    }
    private IEnumerator FakeGround()
    {
        onFakeGround = true;
        transform.position = new Vector3(transform.position.x, feetHit.point.y + 1f, 0);
        g = 0;
        baseMovementVariables._fakeGroundTimer = baseMovementVariables.fakeGroundTime;
        while (baseMovementVariables._fakeGroundTimer > 0 && onFakeGround)
        {
            baseMovementVariables._fakeGroundTimer -= Time.fixedDeltaTime;
            yield return fixedUpdate;
        }
        onFakeGround = false;
    }
    public void ResetPosition()
    {
        rb.velocity = Vector3.zero;
        g = 0;
        transform.position = lastViablePosition;
    }
    public void UpdateRespawnPoint() => lastViablePosition = transform.position;
}
