using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRope : MonoBehaviour
{
    [SerializeField] DistanceJoint2D rope;
    [SerializeField] Transform ropeAnchor;
    [SerializeField] LineRenderer ropeRender;
    [SerializeField] float ropeMaxDistance;
    [SerializeField] LayerMask ceilingLayer;
    RaycastHit2D hit;
    [SerializeField] float ropeY;
    [SerializeField] float ropeX;
    [SerializeField] float ropeLastSling;
    // Returns true if rope is being used
    static public bool usingRope;

    bool ropeUsed;


    // MOUSE VARIABLES, CAN DELETE
    //Vector3 targetPosition;
    //RaycastHit2D aimHit;

    // GIZMOS VARIABLES
    [SerializeField] float ropeAnchorRadius; // For gizmos

    // Start is called before the first frame update
    void Start()
    {
        rope = GetComponent<DistanceJoint2D>();
        rope.enabled = false;
        ropeRender.enabled = false;

        // Fixes a bug where character used rope on pause menu
        PauseMenu.gamePaused = false;

    }

    // Update is called once per frame
    void Update()
    {
        // Variables for rope position;
        Vector2 targetPosition = new Vector2(0, 0);

        Vector2 ropePosition;
        ropePosition.x = ropeAnchor.position.x;
        ropePosition.y = ropeAnchor.position.y;

        // One rope per jump
        if (CharacterMovement.onGround)
            ropeUsed = false;


        if (PauseMenu.gamePaused == false)
            if (CharacterMovement.onGround == false)
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    /* COLLIDER COM CLOSESTPOINT SE FOR PRECISO <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    Collider2D searchCollision = Physics2D.OverlapCircle(ropePosition, 2f, ceilingLayer);
                    // Keeps the collision closest point in a vector
                    if (searchCollision != null)
                        targetPosition = searchCollision.ClosestPoint(ropePosition);
                    */

                    // Creates a 2dRaycast to get the collision rigidbody
                    // Uses ropeX and ropeY to aim the rope hit
                    if (transform.right.x > 0)
                        hit = Physics2D.Raycast(ropePosition, ropePosition + new Vector2(ropeX, ropeY) - ropePosition, ropeMaxDistance, ceilingLayer);
                    else if (transform.right.x < 0)
                        hit = Physics2D.Raycast(ropePosition, ropePosition + new Vector2(-ropeX, ropeY) - ropePosition, ropeMaxDistance, ceilingLayer);

                     
                    //  if it collides with something
                    if (hit.collider != null && hit.point.y > ropePosition.y && ropeUsed == false)
                    {
                        // Only one rope per jump
                        ropeUsed = true;

                        rope.enabled = true;

                        // Connects the joint final position to the rigidbody it hits
                        rope.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                        // Defines the anchor point to the point where it hitted
                        rope.connectedAnchor = new Vector2(hit.point.x, hit.point.y + 0.15f);

                        // Sets rope distance, starts the rop with the size of this vector
                        rope.distance = hit.distance;

                        ropeRender.enabled = true;
                        ropeRender.SetPosition(0, ropeAnchor.position);
                        ropeRender.SetPosition(1, new Vector3(rope.connectedAnchor.x, rope.connectedAnchor.y - 0.15f, 0));
                    }
                }

                // Renders rope while pressing Fire3
                if (Input.GetButton("Fire3") && hit.collider != null)
                {
                    usingRope = true;
                    ropeRender.SetPosition(0, ropeAnchor.position);

                        
                    /* Ainda pode dar jeito
                    if (transform.right.x > 0 && rope.distance < ropeMaxDistance)    // FALTA METER VELOCIDADE AQUI <<<<<<<<<<<<<<<<<<<<<<<<<
                    {
                        if (CharacterMovement.rb.velocity.y > -2)
                            CharacterMovement.rb.position += new Vector2(-0.02f, 0f);

                        else
                        {
                            CharacterMovement.rb.position += new Vector2(-0.07f, -0.025f);
                            rope.distance += 0.1f;

                            //fixe
                        }
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
                        CharacterMovement.rb.AddForce(new Vector2(0f, ropeLastSling));
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
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.92f, 0.015f, 0.1f);
        Gizmos.DrawSphere(ropeAnchor.position, ropeAnchorRadius);
    }
}



/* MOUSE INPUT ROPE
                        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        targetPosition.z = 0;

                        // Raycast from this position to aiming position
                        aimHit = Physics2D.Raycast(transform.position, targetPosition - transform.position, ropeMaxDistance, ceilingLayer);

                        // If the collider has something (if the raycast detected something)
                        if (aimHit.collider != null && aimHit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                        {
                            // Platform above player
                            if (targetPosition.y > playerSize.y)
                            {
                                rope.enabled = true;

                                // Connects the joint final position to the rigidbody it hits
                                rope.connectedBody = aimHit.collider.gameObject.GetComponent<Rigidbody2D>();
                                // Defines the anchor point to the point where it hitted
                                rope.connectedAnchor = aimHit.point - new Vector2(aimHit.collider.transform.position.x, aimHit.collider.transform.position.y);

                                // Sets rope size
                                rope.distance = Vector2.Distance(transform.position, aimHit.point);

                                // Rope rendering
                                ropeRender.enabled = true;
                                ropeRender.SetPosition(0, playerSize);
                                ropeRender.SetPosition(1, aimHit.point);
                            }
                        }
                        */
// Ssearch for a collision in an area