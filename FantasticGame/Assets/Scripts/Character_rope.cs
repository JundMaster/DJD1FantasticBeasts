using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_rope : MonoBehaviour
{
    DistanceJoint2D rope;
    [SerializeField] LineRenderer ropeRender;

    Vector3 targetPosition;
    RaycastHit2D aimHit;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask ceilingLayer;

    [SerializeField] Transform ropeStart;
    [SerializeField] float ropeStartRadius; // For gizmos

    Vector3 playerSize;

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
        playerSize = transform.position + new Vector3(0.1f, 0.6f, 0);

        if (PauseMenu.gamePaused == false)
            if (character_movement.onGround == false)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetPosition.z = 0;

                    //ropeRender.enabled = true;
                    //ropeRender.SetPosition(1, targetPosition);

                    // Raycast from this position to aiming position
                    aimHit = Physics2D.Raycast(transform.position, targetPosition - transform.position, maxDistance, ceilingLayer);

                    // If the collider has something (if the raycast detected something)
                    if (aimHit.collider != null && aimHit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                    {
                        rope.enabled = true;

                        // Connects the joint final position to the rigidbody it hits
                        rope.connectedBody = aimHit.collider.gameObject.GetComponent<Rigidbody2D>();
                        // Defines the anchor point to the point where it hitted
                        rope.connectedAnchor = aimHit.point - new Vector2(aimHit.collider.transform.position.x, aimHit.collider.transform.position.y);

                        rope.distance = Vector2.Distance(transform.position, aimHit.point);

                        // Rope rendering
                        ropeRender.enabled = true;
                        ropeRender.SetPosition(0, playerSize);
                        ropeRender.SetPosition(1, aimHit.point);
                    }

                    else if (aimHit.collider == null)
                    {
                        Collider2D searchCollision = Physics2D.OverlapCircle(playerSize, 1.5f, ceilingLayer);
           

                        if (searchCollision != null)
                            Debug.Log("DETECTED COL");

            
                    }
                }
                // Renderes rope while pressing Fire2
                if (Input.GetButton("Fire2"))
                    ropeRender.SetPosition(0, playerSize);


                if (Input.GetButtonUp("Fire2"))
                {
                    rope.enabled = false;
                    ropeRender.enabled = false;
                }
            }

            else
            {
                rope.enabled = false;
                ropeRender.enabled = false;
            }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.92f, 0.015f, 0.1f);

        Gizmos.DrawSphere(ropeStart.position, ropeStartRadius);
    }
}
