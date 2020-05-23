using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2                             currentVelocity;
    float                               hAxis;
    float                               runSpeed;

    // CROUCHED
    [SerializeField] BoxCollider2D      boxCol;
    [SerializeField] CircleCollider2D   circleCol;
    [SerializeField] Transform          ceilingOverHead;
    bool                                usingCrouch;
    // CROUCHED GET SET
    public bool                         CrouchGetter    { get; private set; }
    public bool                         IsCrouched      { get; private set; }
    
    // POSITION GET SET
    public Vector3                      Position        { get; private set; }


    // GROUNDCHECK ++ JUMP
    bool                                noVelY;
    float                               coyoteCounter;
    bool                                groundedNotFloor;
    [SerializeField] Transform          groundCheck;
    // GROUNDCHECK GET SET
    public bool                         OnGround        { get; private set; }
    float                               jumpTime;
    bool                                jumped;
    float                               lastJumpCounter;
    float                               lastJumpDelay;


    // ROPE
    [SerializeField] Transform          ropeAnchor;
    [SerializeField] Transform          ropeWallCollider;
    [SerializeField] DistanceJoint2D    rope;
    [SerializeField] LineRenderer       ropeRender;
    RaycastHit2D                        ropeHit;
    Vector3                             ropeHitCoords;
    Vector3                             newRopeSprite;
    GameObject                          ropeSprite;
    bool                                minRange;
    bool                                usingRope;
    float                               ropeDelay;
    float                               ropeTimer;
    int                                 ropesLeft;

    // ENEMY HIT
    public bool                         Invulnerable    { get; set; }
    float                               invulnerableTimer;
    float                               invulnerableDelay;
    [SerializeField] SpriteRenderer     spriteRender;
    float                               spriteEnableCounter;
    float                               spriteEnableDelay;

    // Layers
    [SerializeField] LayerMask  ceilingLayer;
    [SerializeField] LayerMask  onGroundLayers;
    [SerializeField] LayerMask  groundedNotFloorLayers;
    [SerializeField] LayerMask  deathTileLayer;
    [SerializeField] LayerMask  ropeStopLayer;
    [SerializeField] LayerMask  enemyLayer;

    // ETC
    private Player              player;
    public Rigidbody2D          Rb { get; private set; }
    Animator                    animator;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rope = GetComponent<DistanceJoint2D>();
        player = FindObjectOfType<Player>();
        ropeSprite = GameObject.FindGameObjectWithTag("ropeSprite");
    }

    void Start()
    {
        rope.enabled        = false;
        ropeRender.enabled  = false;
        noVelY              = false;
        ropeDelay           = 0.25f; // Usable ropes delay
        ropeTimer           = ropeDelay;

        boxCol.enabled      = true; // Crouch colliders
        circleCol.enabled   = false;

        lastJumpDelay = 0.45f; // Timer to jump again
        lastJumpCounter = lastJumpDelay;

        Invulnerable = false;
        invulnerableDelay = 1f;
        invulnerableTimer = invulnerableDelay;
        spriteEnableDelay = 0.1f;
        spriteEnableCounter = spriteEnableDelay;
    }

    void Update()
    {
        // Gets hAxis and sets velocity // Update Variables
        hAxis           = Input.GetAxis("Horizontal");
        Position        = transform.position;
        currentVelocity = Rb.velocity;
        CrouchGetter    = circleCol.enabled;
        // -----------------------------------------------------------------------------------------

        if (!(PauseMenu.gamePaused))
        {
            Movement();
            Grounded();
            Jump();
            Rope();
            NeutralVelY();
            Crouched();
            SpriteRotation();
            EnemyCollision();
            // Sets rigidbody final velocity
            Rb.velocity = currentVelocity;

            // Animator
            animator.SetFloat("absVelX", Mathf.Abs(currentVelocity.x));
            animator.SetFloat("jumpVel", (currentVelocity.y));
            animator.SetBool("groundedNotFloor", groundedNotFloor);
            animator.SetBool("grounded", OnGround);
            animator.SetBool("usingRope", usingRope);
            animator.SetBool("noVelY", noVelY);
            animator.SetBool("crouch", circleCol.enabled);
        }

        // INVULNERABLE  -----------------------------------------------------------------------------
        if (Invulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            spriteEnableCounter -= Time.deltaTime;
        }
        if (spriteEnableCounter < 0)
        {
            spriteRender.enabled = !spriteRender.enabled;
            spriteEnableCounter = spriteEnableDelay;
        }
        if (invulnerableTimer < 0)
        {
            invulnerableTimer = invulnerableDelay;
            Invulnerable = false;
            spriteRender.enabled = true;
        }
        // -----------------------------------------------------------------------------------------


        // COLLISION WITH DEATH TILE ----------------------------------------------------------------
        Collider2D deathTileCheck = Physics2D.OverlapCircle(groundCheck.position, 2f, deathTileLayer);
        if (deathTileCheck != null)
            player.Stats.IsAlive = false;
        // -----------------------------------------------------------------------------------------
        


        // CHECKS IF THE PLAYER IS GROUNDED // FIXES CEILING DOUBLE JUMP BUG // can delete ??
        if (jumped)
            lastJumpCounter -= Time.deltaTime;
        if (lastJumpCounter < 0)
        {
            jumped = false;
            lastJumpCounter = lastJumpDelay;
        }

        // Fixes animation bug on other tiles
        if (Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundedNotFloorLayers)) groundedNotFloor = true;
        else groundedNotFloor = false;
        // -----------------------------------------------------------------------------------------
    }

    void EnemyCollision()
    {
        Collider2D collider = Physics2D.OverlapBox(boxCol.bounds.center, boxCol.bounds.size, 0, enemyLayer);

        if (collider != null)
            if (Invulnerable == false)
            {
                player.Stats.CurrentHP -= 10;
                EnemyHit();
            }
    }

    void EnemyHit()
    {
        currentVelocity.y = 5f;
        Invulnerable = true;
        StartCoroutine(player.CameraShake.Shake(0.015f, 0.04f));
    }


    void Crouched()
    {
        Collider2D collisionTop = Physics2D.OverlapCircle(ceilingOverHead.position, 0.02f, onGroundLayers);

        // Checks if the player pressed crouch
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
        {
            usingCrouch = true;
            IsCrouched = true;
            circleCol.enabled = true;
            boxCol.enabled = false;
        }
        else if (Input.GetKeyUp("s") || Input.GetKeyUp("down"))
        {
            usingCrouch = false;

        }
        // If it's not using crouch AND the collision isn't detecting a roof, stand up
        if (usingCrouch == false && IsCrouched == false)
        {
            
            circleCol.enabled = false;
            boxCol.enabled = true;
        }

        // TEMPORARY << NO ANIMATION
   
            

        if (collisionTop != null)
            IsCrouched = true;
        else
            IsCrouched = false;
    }

    void Grounded()
    {
        // GROUND COLLISION
        float coyoteTime = 0.15f;

        // Ground collision
        Collider2D groundCollision = Physics2D.OverlapCircle(groundCheck.position, 0.05f, onGroundLayers);

        // If there's ground collision and didn't jump for X seconds
        if (groundCollision != null && jumped == false)
        {
            OnGround = true;
        }
        else
            OnGround = false;

        

        // If the character leaves the ground, it has some time (coyoteTime float) to jump
        if (OnGround)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;
    }

    void Jump()
    {
        // JUMP
        float jumpSpeed = 3f;
        float jumpMaxTime = 0.15f;

        // Jump conditions
        if (Input.GetButtonDown("Jump") && coyoteCounter > 0 && Rb.velocity.y < 0.1 && usingCrouch == false && circleCol.enabled == false)
        {
            // If the player jumps, gravityScale is set to 0
            currentVelocity.y = jumpSpeed;
            Rb.gravityScale = 0.0f;
            jumpTime = Time.time;
            OnGround = false;
            jumped = true;
        }
        else if ((Input.GetButton("Jump") && ((Time.time - jumpTime) < jumpMaxTime)))
        {
            // While pressing jump, how much time has passed since jump was pressed
            // Jumps until jumpTime reaches jumpMaxTime
            OnGround = false;
        }
        else
        {
            Rb.gravityScale = 5.0f;
   
        }
    }

    void Rope()
    {
        float ropeMaxDistance   = 1.3f;
        float ropeY             = 0.7f;
        float ropeX             = 0.5f;
        Vector2 ropePosition;
        ropePosition.x          = ropeAnchor.position.x;
        ropePosition.y          = ropeAnchor.position.y;


        if (!(usingRope))
        {
            // Doesn't let the player use the rope too close
            Collider2D notPossibleRope = Physics2D.OverlapCircle(ropePosition, 0.50f, ceilingLayer);
            if (notPossibleRope) minRange = true;
            else minRange = false;
            // Sets ropeSprite to rope anchor position   AND keeps refreshing its position
            if (ropeSprite != null) ropeSprite.SetActive(false);
            newRopeSprite = ropeAnchor.position;
        }
        if (ropeSprite != null) ropeSprite.transform.position = newRopeSprite;

        // Rope timer
        if (rope.enabled == false)
            ropeTimer -= Time.deltaTime;
        if (ropeTimer < 0)
        {
            ropeTimer = ropeDelay;
            ropesLeft = 1;
        }

        // It's possible to use swooping evil if it isn't being used as a platform
        if (OnGround == false && SwoopingEvilPlatform.isAlive == false)
        {
            if (Input.GetButtonDown("Fire3") )
            {
                // Creates a 2dRaycast to get the collision rigidbody
                // Uses ropeX and ropeY to aim the rope hit
                if (transform.right.x > 0)
                    ropeHit = Physics2D.Raycast(ropePosition, ropePosition + new Vector2(ropeX, ropeY) - ropePosition, ropeMaxDistance, ceilingLayer);
                else if (transform.right.x < 0)
                    ropeHit = Physics2D.Raycast(ropePosition, ropePosition + new Vector2(-ropeX, ropeY) - ropePosition, ropeMaxDistance, ceilingLayer);

                //  if it collides with something
                if (ropeHit.collider != null && ropeHit.point.y > ropePosition.y && ropesLeft > 0 && minRange == false)
                {         
                    ropesLeft -= 1;
               
                    rope.enabled = true;

                    // Connects the joint final position to the rigidbody it hits
                    rope.connectedBody = ropeHit.collider.gameObject.GetComponent<Rigidbody2D>();

                    // Defines the anchor point to the point where the rope hitted
                    rope.connectedAnchor = new Vector2(ropeHit.point.x, ropeHit.point.y + 0.15f);
                    ropeHitCoords = new Vector3(ropeHit.point.x, ropeHit.point.y, ropeSprite.transform.position.z);

                    // Sets rope distance, starts the rop with the size of this vector
                    rope.distance = ropeHit.distance;

                    ropeSprite.SetActive(true);
                    ropeRender.enabled = true;       
                }
            }

            // Renders rope while pressing Fire3
            if (Input.GetButton("Fire3") && ropeHit.collider != null && minRange == false)
            {
                usingRope = true;
                ropeRender.SetPosition(0, ropeAnchor.position);

                // If the rope hasn't reached its point, it keeps drawing its self
                if (ropeSprite.transform.position != ropeHitCoords) 
                    newRopeSprite = Vector3.MoveTowards(ropeSprite.transform.position, new Vector3(ropeHitCoords.x -0.05f, ropeHitCoords.y - 0.15f, ropeHitCoords.z), 10f * Time.deltaTime);
                ropeRender.SetPosition(1, new Vector3 (ropeSprite.transform.position.x, ropeSprite.transform.position.y - 0.138f, ropeSprite.transform.position.z));


                Collider2D wallCol = Physics2D.OverlapCircle(ropeWallCollider.position, 0.02f, ropeStopLayer);
                if (wallCol != null)
                    rope.enabled = false;

            }

            if (Input.GetButtonUp("Fire3") && ropeHit.collider != null)
            {
                rope.enabled = false;
                ropeRender.enabled = false;

                // Gives a final boost
                if (usingRope)
                {
                    usingRope = false;
                }
            }
        }
        // Else if the player is grounded
        else
        {
            rope.enabled = false;
            ropeRender.enabled = false;
            usingRope = false;
        }
    }

    void Movement()
    {
        // Running speed
        if (player.UsingShield) runSpeed = 0f;
        if (circleCol.enabled == true && player.UsingShield == true) runSpeed = 0f;
        if (circleCol.enabled == true && player.UsingShield == false) runSpeed = 1f;
        if (circleCol.enabled == false && player.UsingShield == false) runSpeed = 2f;
        // Rope movement
        Vector2 rightBalance = new Vector2(1500f * Time.deltaTime, 0f);
        Vector2 leftBalance = new Vector2(-1500f * Time.deltaTime, 0f);

        // If the character is using a rope, ignore this speed
        if (!(usingRope))
            currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);

        if (usingRope)
        {
            Rb.drag = 0.02f;
            if (Rb.velocity.x < 3 && Rb.velocity.x > -3)
            {
                if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
                {
                    Rb.AddForce(rightBalance);
                }
                if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
                {
                    Rb.AddForce(leftBalance);
                }
            }
        }
        else Rb.drag = 0f;
    }

    void NeutralVelY()
    {
        if (Rb.velocity.y == 0)
            noVelY = true;
        else
            noVelY = false;
    }

    void SpriteRotation()
    {
        // SPRITE ROTATION
        // If velocity is negative and the sprite is positive, rotates the sprite to the left
        if (currentVelocity.x < -0.1f)
        {
            if (transform.right.x > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Else, rotates it back to the original position
        else if (currentVelocity.x > 0.1f)
        {
            if (transform.right.x < 0)
                transform.rotation = Quaternion.identity;
        }
    }

}
