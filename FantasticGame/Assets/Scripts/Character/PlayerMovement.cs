using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2     currentVelocity;
    private float       hAxis;
    private float       runSpeed;

    // CROUCHED
    [SerializeField] private BoxCollider2D      boxCol;
    [SerializeField] private CircleCollider2D   circleCol;
    [SerializeField] private Transform          ceilingOverHead;
    private bool usingCrouch;

    // CROUCHED GET SET
    public bool                         CrouchGetter    { get; private set; }
    public bool                         IsCrouched      { get; private set; }
    
    // POSITION GET SET
    public Vector3                      Position        { get; private set; }

    // GROUNDCHECK ++ JUMP
    private bool                        noVelY;
    private float                       coyoteCounter;
    private bool                        groundedNotFloor;
    [SerializeField] Transform          groundCheck;
    public bool                         OnGround        { get; private set; }
    private float                       jumpTime;
    private bool                        jumped;
    private float                       lastJumpCounter;
    private float                       lastJumpDelay;
    private bool                        GroundedSoundPlayable;

    // ROPE
    [SerializeField] private Transform          ropeAnchor;
    [SerializeField] private Transform          ropeWallCollider;
    [SerializeField] private DistanceJoint2D    rope;
    [SerializeField] private LineRenderer       ropeRender;
    private RaycastHit2D                        ropeHit;
    private Vector3                             ropeHitCoords;
    private Vector3                             newRopeSprite;
    public GameObject                           ropeSprite      { get; set; }
    private bool                                minRange;
    public bool                                 usingRope       { get; private set; }
    private float                               ropeDelay;
    private float                               ropeTimer;
    private int                                 ropesLeft;

    // ENEMY HIT
    public bool                         Invulnerable    { get; set; }
    public float                        invulnerableHP  { get; set; }
    private float                       normalHP;
    private float                       invulnerableTimer;
    private float                       invulnerableDelay;
    [SerializeField] private SpriteRenderer spriteRender;
    private float                       spriteEnableCounter;
    private float                       spriteEnableDelay;

    // WALK SOUND TIMERS
    private float walkSoundCounter;
    private float walkSoundDelay;

    // Layers
    [SerializeField] private LayerMask  ceilingLayer;
    [SerializeField] private LayerMask  onGroundLayers;
    [SerializeField] private LayerMask  groundedNotFloorLayers;
    [SerializeField] private LayerMask  deathTileLayer;
    [SerializeField] private LayerMask  ropeStopLayer;
    [SerializeField] private LayerMask  enemyLayer;

    // ETC
    public Player       player  { get; private set; }
    public Rigidbody2D  Rb      { get; private set; }
    private Animator    animator;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rope = GetComponent<DistanceJoint2D>();
        player = FindObjectOfType<Player>();
        ropeSprite = GameObject.FindGameObjectWithTag("ropeSprite");
        if (ropeSprite != null) ropeSprite.SetActive(false);
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

        // Sounds
        GroundedSoundPlayable = true;
        walkSoundDelay = 0.3f;
        walkSoundCounter = walkSoundDelay;
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
            CollisionDeathTiles();
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
        if (player != null) normalHP = player.Stats.CurrentHP;
        if (Invulnerable == false) invulnerableHP = player.Stats.CurrentHP;
        if (Invulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            spriteEnableCounter -= Time.deltaTime;
            if (player.Stats.CurrentHP < 0) player.Stats.IsAlive = false;
            player.Stats.CurrentHP = invulnerableHP - 30f;
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
            invulnerableHP = player.Stats.CurrentHP;
            player.Stats.CurrentHP = normalHP;
        }
        // -----------------------------------------------------------------------------------------


        // CHECKS IF THE PLAYER IS GROUNDED // FIXES CEILING DOUBLE JUMP BUG //
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


    void CollisionDeathTiles()
    {
        Collider2D deathTileCheck = Physics2D.OverlapCircle(groundCheck.position, 0.1f, deathTileLayer);
        if (deathTileCheck != null)
        {
            if (ropeSprite != null) ropeSprite.SetActive(false);
            player.Stats.IsAlive = false;
        }
    }


    void EnemyCollision()
    {
        if (player.GodMode == false)
        {
            Collider2D collider = Physics2D.OverlapBox(boxCol.bounds.center, boxCol.bounds.size, 0, enemyLayer);

            if (collider != null)
                if (Invulnerable == false)
                {
                    SoundManager.PlaySound(AudioClips.enemyHit); // Plays sound
                    EnemyHit();
                }
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
        if (Input.GetKey("down") && OnGround)
        {
            usingCrouch = true;
            IsCrouched = true;
            circleCol.enabled = true;
            boxCol.enabled = false;
        }
        else if (Input.GetKeyUp("down") && OnGround)
        {
            usingCrouch = false;

        }
        // If it's not using crouch AND the collision isn't detecting a roof, stand up
        if (usingCrouch == false && IsCrouched == false)
        {
            
            circleCol.enabled = false;
            boxCol.enabled = true;
        }
            
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

            // Plays landing sound when lands on the floor
            if (GroundedSoundPlayable == false)
            {
                SoundManager.PlaySound(AudioClips.jumpLanding); // plays sound
                GroundedSoundPlayable = true;
            }
        }
        else
        {
            OnGround = false;
            GroundedSoundPlayable = false;
        }

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
        if (Input.GetButtonDown("Jump") && coyoteCounter > 0 && Rb.velocity.y < 0.1)
        {
            // If the player jumps, gravityScale is set to 0
            usingCrouch = false;
            currentVelocity.y = jumpSpeed;
            Rb.gravityScale = 0.0f;
            jumpTime = Time.time;
            OnGround = false;
            jumped = true;

            SoundManager.PlaySound(AudioClips.jump); // plays sound
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
            Collider2D notPossibleRope = Physics2D.OverlapCircle(ropePosition, 0.40f, ceilingLayer);
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
        if (OnGround == false && SwoopingEvilPlatform.IsAlive == false)
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

                    SoundManager.PlaySound(AudioClips.ropeHit); // plays sound
                }
            }

            // Renders rope while pressing Fire3
            if (Input.GetButton("Fire3") && ropeHit.collider != null && rope.enabled)
            {
                usingRope = true;
                ropeRender.SetPosition(0, ropeAnchor.position);

                // If the rope hasn't reached its point, it keeps drawing its self
                if (ropeSprite.transform.position != ropeHitCoords) 
                    newRopeSprite = Vector3.MoveTowards(ropeSprite.transform.position, new Vector3(ropeHitCoords.x -0.05f, ropeHitCoords.y - 0.15f, ropeHitCoords.z), 10f * Time.deltaTime);
                ropeRender.SetPosition(1, new Vector3 (ropeSprite.transform.position.x, ropeSprite.transform.position.y - 0.138f, ropeSprite.transform.position.z));


                // If the player collides against something
                Collider2D wallCol = Physics2D.OverlapCircle(ropeWallCollider.position, 0.03f, ropeStopLayer);
                if (wallCol != null)
                {
                    if (ropeSprite != null) ropeSprite.SetActive(false);
                    ropeRender.enabled = false;
                    rope.enabled = false;
                }

                // If the player passes the ceining position
                if (player.transform.position.y > ropeHitCoords.y - 0.8f)
                {
                    currentVelocity = -currentVelocity / 2f;
                }
      

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
        if (circleCol.enabled == true && player.UsingShield == false) runSpeed = 0f;
        if (circleCol.enabled == false && player.UsingShield == false) runSpeed = 2f;
        // Rope movement
        Vector2 rightBalance = new Vector2(1500f * Time.deltaTime, 0f);
        Vector2 leftBalance = new Vector2(-1500f * Time.deltaTime, 0f);


        


        // If the character is using a rope, ignore this speed
        if (!(usingRope))
            currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);

        if (usingRope && rope.distance > 0.8f)
        {
            Rb.drag = 0.02f;
            if (Rb.velocity.x < 3 && Rb.velocity.x > -3)
            {
                if (Input.GetKeyDown("right"))
                {
                    Rb.AddForce(rightBalance);
                }
                if (Input.GetKeyDown("left"))
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

    public void StepSound()
    {
        SoundManager.PlaySound(AudioClips.walk); // plays sound
    }

}
