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

    // Crouched
    [SerializeField] BoxCollider2D  collider;
    public bool                     IsCrouched { get; private set; }
    
    // Position
    public Vector3                  Position    { get; private set; }


    // groundChecking variables
    bool                            noVelY;
    float                           coyoteCounter;
    public bool                     onGround    { get; private set; }
    [SerializeField] Transform      groundCheck;
    // Rope

    RaycastHit2D                        ropeHit;
    Collider2D                          notPossibleRope;
    [SerializeField] DistanceJoint2D    rope;
    [SerializeField] Transform          ropeAnchor;
    [SerializeField] LineRenderer       ropeRender;
    [SerializeField] Transform          ropeWallCollider;
    bool                                usingRope;
    bool                                ropeUsed;


    public Rigidbody2D                  rb      { get; private set; }
    Animator                            animator;


    // Layers
    [SerializeField] LayerMask ceilingLayer;
    [SerializeField] LayerMask onGroundLayers;

    [SerializeField] BoxCollider2D boxCollider;
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
    }

    void Update()
    {
        // Gets hAxis and sets velocity // Update Variables
        hAxis = Input.GetAxis("Horizontal");
        Position = transform.position;
        currentVelocity = rb.velocity;
        bool pressCrouch = Input.GetKey("s") || Input.GetKey("down");


        if (!(PauseMenu.gamePaused))
        {
            Movement();
            Grounded();
            Jump();
            Rope();
            NeutralVelY();
            IsCrouched = pressCrouch && onGround ? true : false;
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
        }        
    }
    // CORRIGIR <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    void Crouched()
    {
        if (IsCrouched)
        {
            collider.size = new Vector2(0.29f, 0.30f);
            collider.offset = new Vector2(-0.02f, 0.15f);
        }
        else
        {
            collider.size = new Vector2(0.29f, 0.60f);
            collider.offset = new Vector2(-0.02f, 0.3f);
        }
        
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

        // One rope per jump
        if (onGround)
            ropeUsed = false;
    }

    void Jump()
    {
        // JUMP
        float jumpSpeed = 3f;
        float jumpMaxTime = 0.15f;
        
        // Jump conditions
        if ((Input.GetButtonDown("Jump") && coyoteCounter > 0 && rb.velocity.y < 0.1))
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
        float ropeLastSling = 100f;
        bool canUseRope = true;
        
        Vector2 ropePosition;
        ropePosition.x = ropeAnchor.position.x;
        ropePosition.y = ropeAnchor.position.y;


        notPossibleRope = Physics2D.OverlapCircle(ropePosition, 0.3f, ceilingLayer);

        if (notPossibleRope != null) canUseRope = false;

        if (onGround == false && canUseRope)
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
                if (ropeHit.collider != null && ropeHit.point.y > ropePosition.y && ropeUsed == false)
                {
                    // Only one rope per jump
                    ropeUsed = true;
               
                    rope.enabled = true;

                    // Connects the joint final position to the rigidbody it hits
                    rope.connectedBody = ropeHit.collider.gameObject.GetComponent<Rigidbody2D>();

                    // Defines the anchor point to the point where it rope hitted
                    rope.connectedAnchor = new Vector2(ropeHit.point.x, ropeHit.point.y + 0.15f);

                    // Sets rope distance, starts the rop with the size of this vector
                    rope.distance = ropeHit.distance;

                    ropeRender.enabled = true;
                    ropeRender.SetPosition(0, ropeAnchor.position);
                    ropeRender.SetPosition(1, new Vector3(rope.connectedAnchor.x, rope.connectedAnchor.y - 0.15f, 0));
                    

                    // TESTING ROPE RENDER
                    //ropeRender.SetPosition(1, ropeAnchor.position);
                }
            }

            // Renders rope while pressing Fire3
            if (Input.GetButton("Fire3") && ropeHit.collider != null)
            {
                usingRope = true;
                ropeRender.SetPosition(0, ropeAnchor.position);

                Collider2D wallCol = Physics2D.OverlapCircle(ropeWallCollider.position, 0.02f, onGroundLayers);
                if (wallCol != null)
                    rope.enabled = false;

                // ADD ROPE SIZE // RENDER
                /*
                Vector3 thisRopePosition = ropeAnchor.position;
                if (ropeRender.GetPosition(1) != new Vector3(rope.connectedAnchor.x, rope.connectedAnchor.y - 0.15f, 0))
                {
                    thisRopePosition += new Vector3(0.1f, 0.1f, 0);
                    ropeRender.SetPosition(1, thisRopePosition);
                }
                */


            }

            if (Input.GetButtonUp("Fire3"))
            {
                rope.enabled = false;
                ropeRender.enabled = false;

                // Gives a final boost
                if (usingRope)
                {
                    rb.AddForce(new Vector2(0f, ropeLastSling));
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
        float runSpeed = 2f;
        if (player.usingShield) runSpeed = 0f;
        if (player.usingShield && IsCrouched) runSpeed = 0f;
        if (IsCrouched && !IsCrouched) runSpeed = 1f;
        // Rope movement
        Vector2 rightBalance = new Vector2(1500f * Time.deltaTime, 0f);
        Vector2 leftBalance = new Vector2(-1500f * Time.deltaTime, 0f);

        // If the character is using a rope, ignore this speed
        if (!(usingRope))
            currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);

        else
        {
            if (rb.velocity.x < 3 && rb.velocity.x > -3)
            {
                if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
                {
                    Debug.Log(rightBalance);
                    rb.AddForce(rightBalance);
                }
                if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
                {
                    Debug.Log(leftBalance);
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


    void OnCollisionEnter2D(Collision2D hitInfo)
    {
        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

        if (enemy != null)
        {
            // FALTA METER MOVIMENTO QND LEVA HIT
            player.stats.TakeDamage(25f);
        }
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
