using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_rope : MonoBehaviour
{
    [SerializeField] DistanceJoint2D rope;
    [SerializeField] LineRenderer ropeRender;

    Vector3 targetPosition;
    RaycastHit2D aimHit;

    Vector2 missTargetPosition;
    RaycastHit2D missAimHit;

    [SerializeField] float maxDistance;
    [SerializeField] LayerMask ceilingLayer;

    [SerializeField] Transform ropeStart;
    [SerializeField] float ropeStartRadius; // For gizmos

    Vector3 playerSize;
    Vector2 playerCheck;

    static public bool usingRope;

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
        playerSize = transform.position + new Vector3(0.1f, 0.3f, 0);

        playerCheck.x = playerSize.x;
        playerCheck.y = playerSize.y;

        if (PauseMenu.gamePaused == false)
            if (character_movement.onGround == false)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetPosition.z = 0;

                    // Raycast from this position to aiming position
                    aimHit = Physics2D.Raycast(transform.position, targetPosition - transform.position, maxDistance, ceilingLayer);

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

                    // If player fails the platform, searches for closest collision point
                    else if (aimHit.collider == null)
                    { 
                        // If the player is walking right
                        if (character_movement.rb.velocity.x > 0.01f)
                        {
                            Collider2D searchCollision = Physics2D.OverlapCircle(playerSize, 1.5f, ceilingLayer);

                            missTargetPosition = searchCollision.ClosestPoint(playerCheck);

                            missAimHit = Physics2D.Raycast(playerCheck, missTargetPosition - playerCheck, maxDistance, ceilingLayer);

                            if (searchCollision != null)
                            {
                                rope.enabled = true;

                                // Connects the joint final position to the rigidbody it hits
                                rope.connectedBody = missAimHit.collider.gameObject.GetComponent<Rigidbody2D>();
                                // Defines the anchor point to the point where it hitted
                                rope.connectedAnchor = missAimHit.point - new Vector2(missAimHit.collider.transform.position.x, missAimHit.collider.transform.position.y);

                                // Sets rope size
                                rope.distance = Vector2.Distance(missAimHit.point - new Vector2(1f, 0), missAimHit.point);

                                ropeRender.enabled = true;
                                ropeRender.SetPosition(0, playerSize);
                                ropeRender.SetPosition(1, missTargetPosition);
                            }
                        }

                        // If the player is walking left
                        if (character_movement.rb.velocity.x < 0.01f)
                        {
                            Collider2D searchCollision = Physics2D.OverlapCircle(playerSize, 1.5f, ceilingLayer);

                            missTargetPosition = searchCollision.ClosestPoint(playerCheck);

                            missAimHit = Physics2D.Raycast(playerCheck, missTargetPosition - playerCheck, maxDistance, ceilingLayer);

                            if (searchCollision != null)
                            {
                                rope.enabled = true;

                                // Connects the joint final position to the rigidbody it hits
                                rope.connectedBody = missAimHit.collider.gameObject.GetComponent<Rigidbody2D>();
                                // Defines the anchor point to the point where it hitted
                                rope.connectedAnchor = missAimHit.point - new Vector2(missAimHit.collider.transform.position.x, missAimHit.collider.transform.position.y);

                                // Sets rope size
                                rope.distance = Vector2.Distance(missAimHit.point - new Vector2(1f, 0), missAimHit.point);

                                ropeRender.enabled = true;
                                ropeRender.SetPosition(0, playerSize);
                                ropeRender.SetPosition(1, missTargetPosition);
                            }
                        }
                    }
                }
                // Renderes rope while pressing Fire2
                if (Input.GetButton("Fire2"))
                {
                    usingRope = true;
                    ropeRender.SetPosition(0, playerSize);

                }

                if (Input.GetButtonUp("Fire2"))
                {
                    rope.enabled = false;
                    ropeRender.enabled = false;
                    usingRope = false;

                }
            }

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

        Gizmos.DrawSphere(ropeStart.position, ropeStartRadius);
    }
}
