using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // VARIABLES DECLARATION
    // Movement Variables
    Vector2                         currentVelocity;
    float                           hAxis;
    float                           jumpTime;
    float                           runSpeed;
    /*
    bool                            gotHit;
    float                           gotHitDelay;
    float                           gotHitDelayCounter;
    bool                            invulnerable;
    float                           invulnerableDelay;
    float                           invulnerableCounter;
    float                           temporaryHP;
    */

    // Crouched
    [SerializeField] BoxCollider2D      boxCol;
    [SerializeField] CircleCollider2D   circleCol;
    public bool                     crouchGetter    { get; private set; }
    public bool                     IsCrouched      { get; private set; }
    [SerializeField] Transform      playerScale;
    bool                            usingCrouch;
    [SerializeField] Transform      ceilingOverHead;
    // Position
    public Vector3                  Position    { get; private set; }


    // groundChecking variables
    bool                            noVelY;
    float                           coyoteCounter;
    public bool                     onGround    { get; private set; }
    [SerializeField] Transform      groundCheck;


    // Rope
    RaycastHit2D                        ropeHit;
    bool                                minRange;
    [SerializeField] DistanceJoint2D    rope;
    [SerializeField] Transform          ropeAnchor;
    [SerializeField] LineRenderer       ropeRender;
    [SerializeField] Transform          ropeWallCollider;
    bool                                usingRope;
    float                               ropeDelay;
    float                               ropeTimer;
    int                                 ropesLeft;


    public Rigidbody2D                  rb      { get; private set; }
    Animator                            animator;


    // Layers
    [SerializeField] LayerMask ceilingLayer;
    [SerializeField] LayerMask onGroundLayers;

    [SerializeField] Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rope = GetComponent<DistanceJoint2D>();
        player = GetComponent<Player>();
    }

    void Start()
    {
        rope.enabled = false;
        ropeRender.enabled = false;
        noVelY = false;
        ropeDelay = 0.25f;
        ropeTimer = ropeDelay;

        boxCol.enabled = true;
        circleCol.enabled = false;

        /*
        gotHitDelay = 0.1f;
        gotHitDelayCounter = gotHitDelay;
        invulnerableDelay = 5f;
        invulnerableCounter = invulnerableDelay;
        */
    }

    void Update()
    {
        // Gets hAxis and sets velocity // Update Variables
        hAxis = Input.GetAxis("Horizontal");
        Position = transform.position;
        currentVelocity = rb.velocity;
        crouchGetter = circleCol.enabled;

        if (!(PauseMenu.gamePaused))
        {
            Movement();
            Grounded();
            Jump();
            Rope();
            NeutralVelY();
            Crouched();
            SpriteRotation();
            // Sets rigidbody final velocity
            rb.velocity = currentVelocity;

            // Reference for animatorator movement

            animator.SetFloat("absVelX", Mathf.Abs(currentVelocity.x));
            animator.SetFloat("jumpVel", (currentVelocity.y));
            animator.SetBool("grounded", onGround);
            animator.SetBool("usingRope", usingRope);
            animator.SetBool("noVelY", noVelY);
            animator.SetBool("crouch", circleCol.enabled);
        }        
    }
    // CORRIGIR <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
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


        if (groundCollision != null)
        {
            onGround = true;
        }
        else
            onGround = false;

        

        // If the character leaves the ground, it has some time (coyoteTime float) to jump
        if (onGround)
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
        if ((Input.GetButtonDown("Jump") && coyoteCounter > 0 && rb.velocity.y < 0.1) && usingCrouch == false && circleCol.enabled == false)
        {
            // If the player jumps, gravityScale is set to 0
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 0.0f;
            jumpTime = Time.time;
            onGround = false;
        }
        else if ((Input.GetButton("Jump") && ((Time.time - jumpTime) < jumpMaxTime)))
        {
            // While pressing jump, how much time has passed since jump was pressed
            // Jumps until jumpTime reaches jumpMaxTime
            onGround = false;
        }
        else
        {
            rb.gravityScale = 5.0f;
   
        }
    }

    void Rope()
    {
        float ropeMaxDistance = 1.2f;
        float ropeY = 0.6f;
        float ropeX = 0.6f;
        Vector2 ropePosition;
        ropePosition.x = ropeAnchor.position.x;
        ropePosition.y = ropeAnchor.position.y;

        // Doesn't let the player use the rope too close
        if (!(usingRope))
        {
            Collider2D notPossibleRope = Physics2D.OverlapCircle(ropePosition, 0.35f, ceilingLayer);
            if (notPossibleRope) minRange = true;
            else minRange = false;
        }
        
        // Rope timer
        if (rope.enabled == false)
            ropeTimer -= Time.deltaTime;
        if (ropeTimer < 0)
        {
            ropeTimer = ropeDelay;
            ropesLeft = 1;
        }


        if (onGround == false)
        {
            if (Input.GetButtonDown("Fire3"))
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

                    // Defines the anchor point to the point where it rope hitted
                    rope.connectedAnchor = new Vector2(ropeHit.point.x, ropeHit.point.y + 0.15f);

                    // Sets rope distance, starts the rop with the size of this vector
                    rope.distance = ropeHit.distance;

                    ropeRender.enabled = true;
                    ropeRender.SetPosition(0, ropeAnchor.position);
                    ropeRender.SetPosition(1, new Vector3(rope.connectedAnchor.x, rope.connectedAnchor.y - 0.22f, -0.5f));         
                }
            }

            // Renders rope while pressing Fire3
            if (Input.GetButton("Fire3") && ropeHit.collider != null && minRange == false)
            {
                usingRope = true;
                ropeRender.SetPosition(0, ropeAnchor.position);
                usingRope = true;

                Collider2D wallCol = Physics2D.OverlapCircle(ropeWallCollider.position, 0.02f, onGroundLayers);
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
        if (player.usingShield) runSpeed = 0f;
        if (circleCol.enabled == true && player.usingShield == true) runSpeed = 0f;
        if (circleCol.enabled == true && player.usingShield == false) runSpeed = 1f;
        if (circleCol.enabled == false && player.usingShield == false) runSpeed = 2f;
        // Rope movement
        Vector2 rightBalance = new Vector2(1500f * Time.deltaTime, 0f);
        Vector2 leftBalance = new Vector2(-1500f * Time.deltaTime, 0f);

        // If the character is using a rope, ignore this speed
        if (!(usingRope))
            currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);


        /*
        if (gotHit)
        {
            gotHitDelayCounter -= Time.deltaTime;
            currentVelocity = new Vector2(0f, 3f);
        }
        if (gotHitDelayCounter < 0)
        {
            gotHitDelayCounter = gotHitDelay;
            gotHit = false;
        }

        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            player.stats.CurrentHP = temporaryHP;
        }
        if (invulnerableCounter < 0)
        {
            invulnerable = false;
        }
        */


       
        if (usingRope)
        {
            if (rb.velocity.x < 3 && rb.velocity.x > -3)
            {
                if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
                {
                    rb.AddForce(rightBalance);
                }
                if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
                {
                    rb.AddForce(leftBalance);
                }
            }
        }
    }


    void NeutralVelY()
    {
        if (rb.velocity.y == 0)
            noVelY = true;
        else
            noVelY = false;
    }

    /*
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (!(invulnerable))
            {
                player.stats.TakeDamage(10f);
                temporaryHP = player.stats.CurrentHP;
                invulnerable = true;
                gotHit = true;
            }
        }
    }
    */
    

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
