using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public partial class PlayerController : MonoBehaviour
{
    PhotonView view;
    Health healthComponent;
    #region Variables

    #region Movement Mechanics
    [Header("Additional Mechanics")]
    public bool jumpMechanic;
    public bool dodgeMechanic;
    #endregion

    #region Additional Mechanics Variables
    public BaseMovementVariables baseMovementVariables = new BaseMovementVariables();
    public JumpVariables jumpVariables = new JumpVariables();
    public DodgeVariables dodgeVariables = new DodgeVariables();

    //public SoftPlatformVariables platformVariables = new SoftPlatformVariables();

    #endregion

    #region Player States
    [Header("Player States")]
    public bool isGrounded;
    bool groundCheck;
    public bool onFakeGround;
    public PlayerState playerState;
    public PlayerState previousState;
    #endregion

    #region Primitive Variables
    private float x;
    private float g;
    private float pvX;
    private float y;
    #endregion

    #region Global Variables

    #region Basic Movement
    private float surfaceSlope;
    private float maxVelocity;
    private float speedIncrease;
    private float friction;
    private float airControl;
    [HideInInspector] public bool useGravity = true;
    #endregion

    #region Jump
    private float _jumpBuffer;
    private float _justJumpedCooldown;
    private float _coyoteTimer;
    public int _inAirJumps;
    #endregion

    #region Dodge
    private float _dodgeBuffer;
    private float _justDodgedCooldown;
    public int _inAirDodges;
    #endregion

    #region InAirVariables
    private float distanceToGround;
    private float timeSinceGrounded;
    #endregion

    #endregion

    #region Vectors
    Vector3 groundedForward;
    Vector3 groundedRight;

    Vector3 totalVelocityToAdd;
    Vector3 newRight;
    Vector3 currentRight;

    Vector3 lastViablePosition;
    #endregion

    #region Raycast hits
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public RaycastHit feetHit;
    [HideInInspector] public RaycastHit forwardHit;
    [HideInInspector] public RaycastHit rayToGround;
    #endregion

    #region Events and delegates
    public delegate void PlayerBecameGrounded();
    public event PlayerBecameGrounded playerJustLanded;
    public delegate void PlayerLeftTheGround();
    public event PlayerLeftTheGround playerLeftGround;
    #endregion

    #region Components
    Rigidbody rb;
    CapsuleCollider capCollider;
    #endregion

    #region Other
    private WaitForFixedUpdate fixedUpdate;
    public static PlayerController singleton;
    public LayerMask triggers;
    public LayerMask softPlatform;
    public LayerMask _activeMask;
    #endregion

    #endregion
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        //if (singleton == null)
        //    singleton = this;
        //else
        //    Destroy(gameObject);
    }
    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if(_cameraWork != null)
        {
            if(view.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

        capCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        //moveCamera = GetComponent<MoveCamera>();
        fixedUpdate = new WaitForFixedUpdate();
        friction = baseMovementVariables.inAirFriction;
        airControl = baseMovementVariables.inAirControl;
        g = baseMovementVariables.initialGravity;
        playerState = PlayerState.InAir;
        baseMovementVariables.StartVariables(capCollider);
        healthComponent = GetComponent<Health>();
    }

    void Update()
    {
        if(view.IsMine)
        {
            if (!healthComponent.MovementDisabled) MovementInput();
            if (jumpMechanic && !healthComponent.MovementDisabled) JumpInput();
            if (dodgeMechanic && !healthComponent.MovementDisabled) DodgeInput();
        }
    }

    private void FixedUpdate()
    {
        if(view.IsMine)
        {
            GroundCheck();
            if (playerState != PlayerState.Dodging) Move();
            if (dodgeMechanic) HandleDodgeInput();
            if (useGravity)
            {
                if (jumpMechanic) HandleJumpInput();
                ApplyGravity();
            }
            rb.velocity += totalVelocityToAdd;
            if (rb.velocity.magnitude < baseMovementVariables.minVelocity && x == 0 && isGrounded)        //If the player stops moving set its maxVelocity to walkingSpeed and set its rb velocity to 0
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void SetAnimBool(string valName, bool val) => animator.SetBool(valName, val);

    private void SetAnimTrigger(string valName) => animator.SetTrigger(valName);

    private void SetAnimFloat(string valName, float val) => animator.SetFloat(valName, val, 0.1f, Time.deltaTime);

    public void SetActiveMask(bool checkPlatforms)
    {
        if(checkPlatforms)
        {
            _activeMask = triggers;
        }
        else
        {
            _activeMask = softPlatform;
        }
    }
}
