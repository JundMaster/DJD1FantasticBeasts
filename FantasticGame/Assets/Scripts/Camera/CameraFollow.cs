using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rect cameraTrap;
    [SerializeField] Vector3 offset;
    float camSpeed = 2f;

    // Runs after update()
    private void FixedUpdate()
    {
        // Camera position relative to player
        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;
        Rect rect = CreateRect();


        if (targetPos.x < rect.xMin) rect.xMin = targetPos.x;
        if (targetPos.x > rect.xMax) rect.xMax = targetPos.x;
        if (targetPos.y < rect.yMin) rect.yMin = targetPos.y;
        if (targetPos.y > rect.yMax) rect.yMax = targetPos.y;


        // Variable to know the center of the recrangle
        Vector3 movePos = rect.center;
        movePos.z = transform.position.z;


        Vector3 newPos = movePos - transform.position;
        transform.position += newPos * camSpeed;
    }

    Rect CreateRect()
    {
        Rect rect = cameraTrap;
        rect.position += new Vector2(-rect.width / 2, -rect.height / 2);
        // Camera's x and y
        rect.position += new Vector2(transform.position.x, transform.position.y);

        return rect;
    }

    /*
    private void OnDrawGizmos()
    {
        Rect tempRect = CreateRect();
        Gizmos.color = new Color(1, 1, 1, 0.5f);

        Vector3 p1 = new Vector3(tempRect.xMin, tempRect.yMin, 0);
        Vector3 p2 = new Vector3(tempRect.xMax, tempRect.yMin, 0);
        Vector3 p3 = new Vector3(tempRect.xMax, tempRect.yMax, 0);
        Vector3 p4 = new Vector3(tempRect.xMin, tempRect.yMax, 0);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
    */
}
