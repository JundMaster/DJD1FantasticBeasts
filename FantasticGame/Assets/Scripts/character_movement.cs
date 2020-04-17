using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_movement : MonoBehaviour
{
    public float runSpeed;

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

        rb.velocity = currentVelocity;

        //anim.SetFloat("nome da var", Mathf.Abs(currentVelocity.x));

        if (currentVelocity.x < -0.5f)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        else if (currentVelocity.x > 0.5f)
            transform.rotation = Quaternion.identity;

    }
}
