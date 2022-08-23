using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

#region CLASS DESCRIPTION:
/* 
 * The main purpose of this class is to make calculations based on player input, 
 * that are then passed to the 'CharacterController2D' script which moves the 
 * player object. It also contains quite a lot of logic related to movement. 
 * Most of the important variables are exposed in the inspector, 
 * so you shouldn't need to modify this script much!
 * 
 * NOTES: This script needs to be attached to the 'Player' object 
 * in order for it to function correctly. That object should also 
 * have the tag: Player
*/
#endregion

public class PlayerMovement : MonoBehaviour
{
    /////////////////////////////////////////////////////////////

    #region INITIALIZATIONS:
    private CharacterController2D _controller;
    [Header("INITIALIZATIONS:")]
    public GameObject playerGFX;
    #endregion

    #region PUBLIC FEILDS:
    [Header("MOVEMENT:")]
    public float gravity = -25f;
    public float moveSpeed = 3f;
    public AnimationCurve accelerationCurve;
    public float groundTurnSpeed = 20.0f;
    public float airTurnSpeed = 5.0f;
    [Header("JUMPING:")] 
    public bool doubleJumpOn;
    public bool wallJumpOn;
    public float jumpHeight = 2f;
    public float doubleJumpMod = 0.5f;
    public float minWallJumpHeight = 0.5f;
    public float wallJumpWaitTime = 0.5f;
    public float wallJumpForce = 2.5f;
    public LayerMask wallJumpMask;
    [Header("GLIDING:")]
    public bool glideOn;
    public float glideMod = 0.05f;
    public KeyCode glideButton;
    [Header("SHOOTING:")]
    public bool shootOn;
    [Header("MISC:")]
    public KeyCode cycleInventoryButton;
    public bool debuggingRays;
    public float knockBackMod = 5f;
    public float bounceHeight = 1f;
    public Animator anim;
    #endregion

    #region NON-SERIALIZED PUBLIC FIELDS:
    [System.NonSerialized]
    public int _movementDirection; // -1 = left, 0 = stationary, 1 = right.
    #endregion

    #region PRIVATE:
    private Vector2 _direction;
    private Vector3 _velocity;
    private int _jumpCounter;
    private bool _canDoubleJump;
    private bool _canWallJump;
    private bool wallJumping;
    private int _maxJumps = 2; 
    private Collider2D _boxCol;
    private Inventory _inventory;
    private GameObject _tempMovingPlatform;
    //private Vector3 _movingPlatformVelocity = Vector3.zero;
    private enum GroundType
    {
        CollapsablePlatform,
        SpikeTrap,
        MovingPlatform,
        Other,
    }
    private GroundType _groundType;
    #endregion

    #region PRIVATE POSITIONS:
    Vector3 _bottomLeft;
    Vector3 _bottomMid;
    Vector3 _bottomRight;
    Vector3 _topLeft;
    Vector3 _topMid;
    Vector3 _topRight;
    Vector3 _leftTop;
    Vector3 _leftMid;
    Vector3 _leftBottom;
    Vector3 _rightTop;
    Vector3 _rightMid;
    Vector3 _rightBottom;
    #endregion

    /////////////////////////////////////////////////////////////
    
    void Awake()
    {
        // Get references to other components.
        _controller = GetComponent<CharacterController2D>();
        _boxCol = GetComponent<BoxCollider2D>();
        _inventory = GetComponent<Inventory>();

        anim = GameObject.Find("Player_sprite").GetComponent<Animator>();
        playerGFX = GameObject.Find("Player_GFX");
    }

    void Update()
    {
        float delta = Time.deltaTime;

        RaycastingPoints();
        InitialMovementCalculations(delta);

        // Check to see if we're in contact with a wall so that we can wall-jump.
        _canWallJump = _controller.collisionState.right || _controller.collisionState.left;
         
        // If we're on the ground, reset the jump counter for the double jump.
        if (_controller.isGrounded)
            _jumpCounter = 0;

        // If the player presses the jump button, call the 'Jump()' function.
        if (Input.GetButtonDown("Jump") && _jumpCounter < _maxJumps && (_controller.isGrounded || _canDoubleJump))
            Jump();

        // Add gravity to the '_velocity' variable.
        _velocity.y += gravity * delta * (Input.GetKey(glideButton) && glideOn && _velocity.y < 0 ? glideMod : 1f);

        // Pass the previously calculated '_velocity' to the 
        // 'CharacterController2D' script to process movement.
        _controller.move(_velocity * delta);
        _velocity = _controller.velocity;

        Flip();
        if (_direction.x == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    #region MOVEMENT FUNCTIONS:
    void InitialMovementCalculations(float delta)
    {
        // If the player is pressing the left or right button, call the flip function.
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.0f)
            Flip();
        // Calculate direction based on player input.
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _direction = _direction.normalized;

        //if (_controller.isGrounded)       
            //_velocity.y = 0;

        GetGroundType();
        
        // Set x velocity with some smoothing to simulate momentum.
        if (!wallJumping)
        {
            var smoothedMovementFactor = _controller.isGrounded ? groundTurnSpeed : airTurnSpeed;
            _velocity.x = Mathf.Lerp(_velocity.x, _direction.x * moveSpeed, 
                                    accelerationCurve.Evaluate(smoothedMovementFactor * delta));
        }

        // Set the 'movementDirection' variable. Used by other functions 
        // to tell which direction we're moving in. Read from velocity
        // rather than input.
        if (_controller.velocity.x < -0.1)
            _movementDirection = -1;
        else if (_controller.velocity.x > 0.1)
            _movementDirection = 1;
        else
            _movementDirection = 0;
    }

    // Finds out what we're standing on, if anything... 
    // It also sends signals to some ground types so
    // that they can react to the player standing on them.
    void GetGroundType()
    {
        RaycastHit2D hit = Physics2D.Raycast(_bottomMid, Vector3.down, 0.1f, wallJumpMask);
        if (_controller.collisionState.below)
        {
            // Here's an example of a code block that sends a signal to the platform we're standing on. In this case it makes the platform collapse.
            if (hit && hit.collider.CompareTag("Collapsable Platform"))
            {
                _groundType = GroundType.CollapsablePlatform;
                //hit.transform.gameObject.GetComponent<CollapsablePlatform>().playerOnPlatform = true; // Use this to activate stuff on the platform object when the player stands on it. 
            }
            // Here's another.
            if (hit && hit.collider.CompareTag("Spike Trap"))
            {
                _groundType = GroundType.SpikeTrap;
                //hit.transform.gameObject.GetComponent<SpikeTrap>().TriggerSpikeTrap(); // Use to trigger the wait timer on the spike trap!
            }
            // Moving platforms are a special case where we need to parent the player to the platform
            // so that they move smoothly along with it, but we only want them to be parented 
            // while the player is standing on the platform.
            if (hit && hit.collider.CompareTag("Moving Platform"))
            {
                _groundType = GroundType.MovingPlatform;
                if (!_tempMovingPlatform)
                {
                    _tempMovingPlatform = hit.transform.gameObject;
                    transform.SetParent(hit.transform);
                }
            }
            else
            {
                // This un-parents us from the moving platform when we jump/step off it.
                if (_tempMovingPlatform)
                {
                    transform.SetParent(null);
                    _tempMovingPlatform = null;
                }
                // Set the '_groundType' to 'Other' (i.e. not important).
                if (hit)
                {
                    _groundType = GroundType.Other;
                }
            }
        }
        // This un-parents us from the moving platform when they jump/step off it.
        else if (_tempMovingPlatform)
        {
            transform.SetParent(null);
            _tempMovingPlatform = null;
        }
       
    }
    void Flip()
    {
        if (playerGFX != null)
        {
            // Check to see which direction we're moving in and flip the player sprite if necessary.
            if (_direction.x < 0 && playerGFX.transform.localScale.x > 0 ||
                _direction.x > 0 && playerGFX.transform.localScale.x < 0)
            {
                playerGFX.transform.localScale = new Vector3(-playerGFX.transform.localScale.x, //x
                                                            playerGFX.transform.localScale.y,   //y
                                                            playerGFX.transform.localScale.z);  //z
            }
        }
        else
        {
            // Tell the user that they haven't assigned the player's gfx.
            Debug.Log("ERROR: You need to assign the GameObject that " +
                        "is the parent of your sprites to the 'Player GFX' " +
                        "field of the 'PlayerMovement' script to get your " +
                        "sprites to flip!");
        }
    }

    // Bounces the player upwards when they stomp an enemy.
    public void Bounce()
    {
        _velocity.y = Mathf.Sqrt(2f * bounceHeight * -gravity);
    }
    #endregion

    #region JUMPING FUNCTIONS:
    void Jump()
    {
        // Wall-jumping conditions. Casts rays from the bottom of the controller
        // to see if we're high enough off the ground to wall jump: 
        if (_canWallJump && 
            wallJumpOn &&
            !_controller.isGrounded && 
            (Physics2D.Raycast
            (_bottomLeft, Vector3.down, Mathf.Infinity, wallJumpMask).distance >= minWallJumpHeight ||
            Physics2D.Raycast
            (_bottomMid, Vector3.down, Mathf.Infinity, wallJumpMask).distance >= minWallJumpHeight ||
            Physics2D.Raycast
            (_bottomRight, Vector3.down, Mathf.Infinity, wallJumpMask).distance >= minWallJumpHeight))        
        {
            WallJump();
        }
        else
        {
            // Check to see if we're jumping from the ground.
            if (!Input.GetKey(KeyCode.DownArrow) && _controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                _jumpCounter++;
                if (_controller.isGrounded)
                    _canDoubleJump = true;
            }
            // If we're double jumping, apply a fraction of the force 
            // of a normal jump (amount determined by 'doubleJumpMod' variable).
            else if (!Input.GetKey(KeyCode.DownArrow) && _jumpCounter < _maxJumps && 
                    _canDoubleJump && doubleJumpOn)
            {
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity * doubleJumpMod);
                _jumpCounter++;
                _canDoubleJump = _jumpCounter < _maxJumps ? true : false;
            }
        }
    }
    void WallJump()
    {
        // Check to see which side we're colliding with, 
        // and then add some force to push in the opposite direction.
        if (_controller.collisionState.right)
        {
            _velocity = new Vector3(jumpHeight * -wallJumpForce,                            //x
                                    Mathf.Sqrt(2f * jumpHeight * -gravity * doubleJumpMod), //y    
                                    0f);                                                    //z
        }
        else if (_controller.collisionState.left)
        {
            _velocity = new Vector3(jumpHeight * wallJumpForce, 
                                    Mathf.Sqrt(2f * jumpHeight * -gravity * doubleJumpMod), 
                                    0f);
        }
        StartCoroutine(WallJumpWait());
    }
    #endregion

    #region RAYCASTING:
    // Calculates the positions from which the various rays will be cast.
    void RaycastingPoints()
    {
        // Top points:
        _topLeft = new Vector3(transform.position.x - (_boxCol.bounds.size.x * 0.5f),       // x
                                    transform.position.y + (_boxCol.bounds.size.y * 0.5f),  // y
                                    transform.position.z);                                  // z
        _topMid = new Vector3(transform.position.x,
                                    transform.position.y + (_boxCol.bounds.size.y * 0.5f),
                                    _boxCol.bounds.center.z);
        _topRight = new Vector3(transform.position.x + (_boxCol.bounds.size.x * 0.5f),
                                    transform.position.y + (_boxCol.bounds.size.y * 0.5f),
                                    _boxCol.bounds.center.z);
        // Bottom points:
        _bottomLeft = new Vector3(transform.position.x - (_boxCol.bounds.size.x * 0.5f),    
                                    transform.position.y - (_boxCol.bounds.size.y * 0.5f),   
                                    transform.position.z);                                  
        _bottomMid = new Vector3(transform.position.x, 
                                    transform.position.y - (_boxCol.bounds.size.y * 0.5f), 
                                    _boxCol.bounds.center.z);
        _bottomRight = new Vector3(transform.position.x + (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y - (_boxCol.bounds.size.y * 0.5f), 
                                    _boxCol.bounds.center.z);
        // Left points:
        _leftMid = new Vector3(transform.position.x - (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y, 
                                    transform.position.z);
        _leftTop = new Vector3(transform.position.x - (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y + (_boxCol.bounds.size.y * 0.5f), 
                                    transform.position.z);
        _leftBottom = new Vector3(transform.position.x - (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y - (_boxCol.bounds.size.y * 0.5f), 
                                    transform.position.z);
        // Right points:
        _rightMid = new Vector3(transform.position.x + (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y, 
                                    transform.position.z);
        _rightTop = new Vector3(transform.position.x + (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y + (_boxCol.bounds.size.y * 0.5f), 
                                    transform.position.z);
        _rightBottom = new Vector3(transform.position.x + (_boxCol.bounds.size.x * 0.5f), 
                                    transform.position.y - (_boxCol.bounds.size.y * 0.5f), 
                                    transform.position.z);

        // These are debug Rays that are used to check whether 
        // the points above are being positioned correctly.
        if (debuggingRays)
        {
            Debug.DrawRay(_bottomLeft, Vector3.down, Color.red);
            Debug.DrawRay(_bottomMid, Vector3.down, Color.red);
            Debug.DrawRay(_bottomRight, Vector3.down, Color.red);
            Debug.DrawRay(_topLeft, Vector3.up, Color.red);
            Debug.DrawRay(_topMid, Vector3.up, Color.red);
            Debug.DrawRay(_topRight, Vector3.up, Color.red);
            Debug.DrawRay(_leftTop, Vector3.left, Color.red);
            Debug.DrawRay(_leftMid, Vector3.left, Color.red);
            Debug.DrawRay(_leftBottom, Vector3.left, Color.red);
            Debug.DrawRay(_rightTop, Vector3.right, Color.red);
            Debug.DrawRay(_rightMid, Vector3.right, Color.red);
            Debug.DrawRay(_rightBottom, Vector3.right, Color.red);
        }
    }
    #endregion

    #region INVENTORY:
    // Currently empty, here if you need it though! Most of the inventory
    // related stuff is handled by the 'Inventory' script itself.
    #endregion

    #region CO-ROUTINES:
    // A wait timer for the wall-jump. You'll want to balance 'wallJumpWaitTime' 
    // with the force you're applying to the wall-jump itself ('wallJumpForce').
    IEnumerator WallJumpWait()
    {
        wallJumping = true;
        yield return new WaitForSeconds(wallJumpWaitTime);
        wallJumping = false;
    }
    #endregion
}

