using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // VARIABLES DECLARATION
    // Moving Variables
    float hAxis;
    [SerializeField] float runSpeed = 2.5f;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpMaxTime;
    float jumpTime;
    [SerializeField] float coyoteTime = 0.165f;
    float coyoteCounter;

    // groundChecking variables
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayers;
    public static bool onGround;

    bool jumpClicked;
    bool jumpBeingClicked;

    static public Rigidbody2D rb;
    Animator anim;

    // FOR GIZMOS
    [SerializeField] float groundCheckRadius;

    void Start()
    {
        // Moving formula
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // MOVEMENT INPUTS
        // Gets hAxis and sets
        hAxis = Input.GetAxis("Horizontal");
        jumpClicked = Input.GetButtonDown("Jump");
        jumpBeingClicked = Input.GetButton("Jump");
        
        // MOVEMENT
        Vector2 currentVelocity = rb.velocity;
        Vector2 rightBalance = new Vector2(1500f * Time.deltaTime, 0f);
        Vector2 leftBalance = new Vector2(-1500f * Time.deltaTime, 0f);

        // If the character is using a rope, ignore this speed
        if (!(CharacterRope.usingRope))
            currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);
        else
            if (rb.velocity.x < 2 && rb.velocity.x > -2)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Debug.Log(rightBalance);
                    rb.AddForce(rightBalance);
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log(leftBalance);
                    rb.AddForce(leftBalance);
                }
            }

        // GROUND COLLISION
        // Ground collision -> Checks if groundCheck position + 0.05f circle radius is in contact with the floor
        Collider2D groundCollision = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayers);
        onGround = groundCollision != null;

        // If the character leaves the ground, it has some time (coyoteTime float) to jump
        if (onGround)
        {
            coyoteCounter = coyoteTime;
        }
        else
            coyoteCounter -= Time.deltaTime;

        // JUMP
        // Jump conditions
        if ((jumpClicked) && coyoteCounter > 0)
        {
            // If the player jumps, gravityScale is set to 0
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 0.0f;
            jumpTime = Time.time;

        }
        else if ((jumpBeingClicked) && ((Time.time - jumpTime) < jumpMaxTime))
        {
            // While pressing jump, how much time has passed since jump was pressed
            // Jumps until jumpTime reaches jumpMaxTime
        }
        else
        {
            rb.gravityScale = 5.0f;
        }

        // Sets rigidbody final velocity
        rb.velocity = currentVelocity;


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

        // Reference for animator movement
        anim.SetFloat("absVelX", Mathf.Abs(currentVelocity.x));
        anim.SetFloat("jumpVel", (currentVelocity.y));
        anim.SetBool("grounded", onGround);

    }

    // Draws groundCheck RADIUS CIRCLE on screen  ( can delete )
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}
