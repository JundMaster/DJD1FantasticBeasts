using UnityEngine;

public class camera_follow : MonoBehaviour
{
    // VARIABLES DECLARATION
    public Transform target;
    public float camSpeed = 3f;
    public Vector3 offset;
    Vector3 TempOffset;


    // Runs after update()
    private void LateUpdate()
    {
        // Camera position relative to player
        Vector3 desiredPosition = target.position + offset;
        // Linear interpolation between current position and desired position in t time
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, camSpeed*Time.deltaTime);
        transform.position = smoothedPosition;


        // New Camera Pos
        TempOffset = new Vector3(0, -1, -1);
        // Gets vAxis
        float vAxis = Input.GetAxis("Vertical");
        // If the player isn't moving and pressed down
        if (vAxis < 0) // Isn't moving >> missing
        {
            desiredPosition = target.position + TempOffset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, camSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }


    }
}
