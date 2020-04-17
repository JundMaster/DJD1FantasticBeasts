using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_movement : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    float jumpTime;
    public float jumpMaxTime;

    //public float groundCheckRadius;

    public Transform groundCheck;
    public LayerMask groundLayers;
    

    Rigidbody2D rb;

    Animator anim;

    // Update is called once per frame
    void Start()
    {
        // Moving formula
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
        // Moves the player
        float hAxis = Input.GetAxis("Horizontal");

        Vector2 currentVelocity = rb.velocity;

        currentVelocity = new Vector2(runSpeed * hAxis, currentVelocity.y);


        

        Collider2D groundCollision = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayers);

        bool onGround = groundCollision != null;

        if ((Input.GetButtonDown("Jump")) && (onGround))
        {
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 0.0f;

            jumpTime = Time.time;
        }
        else if ((Input.GetButton("Jump")) && ((Time.time - jumpTime) < jumpMaxTime))
        {
            
        }
        else
        {
            rb.gravityScale = 5.0f;
        }

        rb.velocity = currentVelocity;

        //anim.SetFloat("nome da var", Mathf.Abs(currentVelocity.x));

        if (currentVelocity.x < -0.5f)
        {
            if (transform.right.x > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else if (currentVelocity.x > 0.5f)
        {
            if (transform.right.x < 0)
                transform.rotation = Quaternion.identity;
        }

        

    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }*/
}
