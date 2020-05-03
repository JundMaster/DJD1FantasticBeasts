using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_movement : MonoBehaviour
{
    // VARIABLES DECLARATION
    // Moving Variables
    float hAxis;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpMaxTime;
    float jumpTime;
    Vector2 currentVelocity;

    // groundChecking variables
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float groundCheckRadius; // For gizmos

    public static bool onGround;
    [SerializeField] bool jumpClicked;


    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        // Moving formula
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // MOVEMENT
        Vector2 currentVelocity = rb.velocity;
        currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);

        // Ground collision -> Checks if groundCheck position + 0.05f circle radius is in contact with the floor
        Collider2D groundCollision = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayers);
        onGround = groundCollision != null;

        // JUMP
        // Jump conditions
        if ((jumpClicked) && (onGround))
        {
            // If the player jumps, gravityScale is set to 0
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 0.0f;
            jumpTime = Time.fixedTime;
        }
        else if ((jumpClicked) && ((Time.fixedTime - jumpTime) < jumpMaxTime))
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
    }

    void Update()
    {
        // MOVEMENT
        // Gets hAxis and sets currentVelocity for rigidbody
        hAxis = Input.GetAxis("Horizontal");
        jumpClicked = Input.GetButton("Jump");
        currentVelocity = rb.velocity;

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
