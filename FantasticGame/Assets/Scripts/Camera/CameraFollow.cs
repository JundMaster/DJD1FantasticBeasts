using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PlayerMovement          playerMove;

    [SerializeField] Rect           cameraTrap;
    [SerializeField] Vector3        offset;
    [SerializeField] float          feedBackLoop = 1f;

    [SerializeField] Vector3        maxLevelRangeXmax;
    [SerializeField] Vector3        maxLevelRangeXmin;
    [SerializeField] Vector3        maxLevelRangeYmin;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMovement>();

    }

    private void Start()
    {
        /*
        maxLevelRangeYmin = new Vector3(0f, -1f, 0f);
        */

        maxLevelRangeXmax = new Vector3(111111f, 0f, 0f);
        maxLevelRangeXmin = new Vector3(-2.5f, 0f, 0f);

        //maxLevelRangeYmin = new Vector3(0f, -100000f, 0f);
    }

    private void FixedUpdate()
    {
        playerMove = FindObjectOfType<PlayerMovement>();

        if (transform.position.x + 2.5f >= maxLevelRangeXmax.x)
            transform.position = new Vector3(maxLevelRangeXmax.x - 2.5f, transform.position.y, transform.position.z);

        if (transform.position.x - 2.5f <= maxLevelRangeXmin.x)
            transform.position = new Vector3(maxLevelRangeXmin.x + 2.5f, transform.position.y, transform.position.z);

        //if (transform.position.y <= maxLevelRangeYmin.x)
        //    transform.position = new Vector3(transform.position.x, maxLevelRangeYmin.y + 1f , transform.position.z);

        if (playerMove.Position.x < maxLevelRangeXmax.x)
        {
            // playerMove Pos
            Vector3 targetPos = playerMove.Position + offset;
            targetPos.z = transform.position.z;

            Rect rect = CreateRect();


            if (targetPos.x < rect.xMin) rect.xMin = targetPos.x;
            else if (targetPos.x > rect.xMax) rect.xMax = targetPos.x;
            if (targetPos.y < rect.yMin) rect.yMin = targetPos.y;
            else if (targetPos.y > rect.yMax) rect.yMax = targetPos.y;

            if (playerMove.OnGround)
                rect.yMin = targetPos.y - 0.1f;


            // Center of rectangle
            Vector3 movePosition = rect.center;
            movePosition.z = transform.position.z;


            // Cam direction
            Vector3 camDirection = movePosition - transform.position;

            // Cam movement

            transform.position += camDirection * feedBackLoop;

        }


    }

    Rect CreateRect()
    {
        Rect rect = cameraTrap;
        rect.position += new Vector2(-rect.width / 2, -rect.height / 2);
        // Camera's x and y
        rect.position += new Vector2(transform.position.x, transform.position.y);

        return rect;
    }

    
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
    
}