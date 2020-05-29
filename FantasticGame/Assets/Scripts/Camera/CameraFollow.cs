using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PlayerMovement          p1;
    Vector3                         targetPos;

    // CAMERA MOVEMENT
    Rect                            cameraTrap;
    Vector3                         offset = new Vector3(0f, 0.7f, 0f);
    float                           feedBackLoop = 0.2f;
    new Vector3                     originalOffset;

    // CAMERA RANGES
    [SerializeField] float          maxLevelRangeXmin;
    [SerializeField] float          maxLevelRangeXmax;
    bool                            minRange;
    bool                            maxRange;


    // WHEN LOOKING UP OR DOWN
    float                           lookingCounter;
    float                           lookingDelay;

    private void Awake()
    {
        p1 = FindObjectOfType<PlayerMovement>();

    }

    private void Start()
    {


        lookingDelay = 0.5f;
        lookingCounter = lookingDelay;

        originalOffset = offset;
    }

    private void FixedUpdate()
    {
        p1 = FindObjectOfType<PlayerMovement>();

        
        // IF PLAYER LOOKS UP OR DOWN -------------------------------------------------------------------------------
        if (Input.GetKey("up") && offset.y > 0.6f)
        {
            lookingCounter -= Time.deltaTime;
            if (lookingCounter < 0)
                if (offset.y < 1.5f)
                    offset.y += 3 * Time.deltaTime;
            if (p1.Rb.velocity.x != 0)
                if (p1.Rb.velocity.y > -5)
                {
                    offset.y = originalOffset.y;
                    lookingCounter = lookingDelay;
                }
        }
        else if (Input.GetKey("down"))
        {
            lookingCounter -= Time.deltaTime;
            if (lookingCounter < 0)
                if (offset.y > 0f)
                    offset.y -= 3 * Time.deltaTime;
            if (p1.Rb.velocity.x != 0)
                if (p1.Rb.velocity.y > -5)
                {
                    offset.y = originalOffset.y;
                    lookingCounter = lookingDelay;
                }
        }
        else // WHENEVER SPEED IS > -5, SETS CAMERA TO ITS NORMAL STATE
            if (p1.Rb.velocity.y > -5)
            {
                offset.y = originalOffset.y;
                lookingCounter = lookingDelay;
                feedBackLoop = 0.2f;
            }
        // ----------------------------------------------------------------------------------------------------------


        // CAMERA WHEN PLAYER VELOCITY IS TOO HIGH ------------------------------------------------------------------
        if (minRange == false && maxRange == false)
        {
            if (p1.Rb.velocity.y < -5)
            {
                if (offset.y > -2f)
                    offset.y -= 3 * Time.deltaTime;
                feedBackLoop = 0.5f;
            }
        }
        // ----------------------------------------------------------------------------------------------------------


        // MAX CAMERA POSITIONS -------------------------------------------------------------------------------------
        if (transform.position.x + 2.5f >= maxLevelRangeXmax)
        {
            maxRange = true;
            transform.position = new Vector3(maxLevelRangeXmax - 2.5f, transform.position.y, transform.position.z);
        }
        else maxRange = false;

        if (transform.position.x - 2.5f <= maxLevelRangeXmin)
        {
            minRange = true;
            transform.position = new Vector3(maxLevelRangeXmin + 2.5f, transform.position.y, transform.position.z);
        }
        else minRange = false;



        // CAMERA MOVEMENT ------------------------------------------------------------------------------------------
        if (p1.Position.x < maxLevelRangeXmax)
        {
            // p1 Pos
            targetPos = p1.Position + offset;
            targetPos.z = transform.position.z;

            Rect rect = CreateRect();


            if (targetPos.x < rect.xMin) rect.xMin = targetPos.x;
            else if (targetPos.x > rect.xMax) rect.xMax = targetPos.x;
            if (targetPos.y < rect.yMin) rect.yMin = targetPos.y;
            else if (targetPos.y > rect.yMax) rect.yMax = targetPos.y;

            if (p1.OnGround)
                rect.yMin = targetPos.y - 0.1f;

            // Center of rectangle
            Vector3 movePosition = rect.center;
            movePosition.z = transform.position.z;

            // Cam direction
            Vector3 camDirection = movePosition - transform.position;

            // Cam movement
            transform.position += camDirection * feedBackLoop;
        }
        // ----------------------------------------------------------------------------------------------------------

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

        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawLine(new Vector3(maxLevelRangeXmin, -30f, 0f), new Vector3(maxLevelRangeXmin, 30f, 0f));
        Gizmos.DrawLine(new Vector3(maxLevelRangeXmax, -30f, 0f), new Vector3(maxLevelRangeXmax, 30f, 0f));
    }
    
}