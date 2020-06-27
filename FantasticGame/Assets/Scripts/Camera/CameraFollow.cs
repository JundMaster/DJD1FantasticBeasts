using UnityEngine;
using UnityEngine.SceneManagement;

sealed public class CameraFollow : MonoBehaviour
{
    private PlayerMovement          p1;
    private Vector3                 targetPos;

    // CAMERA MOVEMENT
    private Rect        cameraTrap;
    private Vector3     offset = new Vector3(0f, 0.7f, 0f);
    private float       feedBackLoop = 0.2f;
    private Vector3     originalOffset;

    // CAMERA RANGES
    [SerializeField] private float maxLevelRangeXmax;
    [SerializeField] private float canMoveCamera;
    private bool maxRange;
    public bool cameraMoving { get; private set; }
    public static float WinningRange { get; private set; }

    // LOOKING TIMER
    private float lookingCounter;
    private float lookingDelay;

    // BOSS CAMERAS
    Vector3 bossScreen;
    Vector3 currentPos;

    private void Start()
    {
        p1 = FindObjectOfType<PlayerMovement>();

        lookingDelay    = 0.5f;
        lookingCounter  = lookingDelay;

        originalOffset = offset;

        WinningRange = maxLevelRangeXmax;

        bossScreen = new Vector3(0, 0, 0);
        currentPos = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<PlayerMovement>();
        }

        // IF PLAYER LOOKS UP OR DOWN -------------------------------------------------------------------------------
        if (p1 != null)
        {
            if (p1.player.LookingUp && offset.y > 0.6f)
            {
                lookingCounter -= Time.fixedDeltaTime;
                if (lookingCounter < 0)
                    if (offset.y < 1.5f)
                        offset.y += 3 * Time.fixedDeltaTime;
                if (p1.Rb.velocity.x != 0)
                    if (p1.Rb.velocity.y > -12)
                    {
                        offset.y = originalOffset.y;
                        lookingCounter = lookingDelay;
                    }
            }
            else if (p1.player.LookingDown)
            {
                lookingCounter -= Time.fixedDeltaTime;
                if (lookingCounter < 0)
                    if (offset.y > 0f)
                        offset.y -= 3 * Time.fixedDeltaTime;
                if (p1.Rb.velocity.x != 0)
                    if (p1.Rb.velocity.y > -12)
                    {
                        offset.y = originalOffset.y;
                        lookingCounter = lookingDelay;
                    }
            }
            else // WHENEVER SPEED IS > -12, SETS CAMERA TO ITS NORMAL STATE
                if (p1.Rb.velocity.y > -12)
            {
                offset.y = originalOffset.y;
                lookingCounter = lookingDelay;
                feedBackLoop = 0.2f;
            }
            // ----------------------------------------------------------------------------------------------------------
        }

        // CAMERA WHEN PLAYER VELOCITY IS TOO HIGH ------------------------------------------------------------------
        if (maxRange == false) // && minRange == false 
        {
            if (p1 != null)
            {
                if (p1.Rb.velocity.y < -12)
                {
                    if (offset.y > -2f)
                        offset.y -= 3 * Time.fixedDeltaTime;
                    feedBackLoop = 0.5f;
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------


        // MAX CAMERA POSITIONS -------------------------------------------------------------------------------------
        if (transform.position.x >= maxLevelRangeXmax) //maxLevelRangeXmax
        {
            maxRange = true;
            transform.position = new Vector3(maxLevelRangeXmax, transform.position.y, transform.position.z);
        }
        else maxRange = false;


        // CAMERA MOVEMENT ------------------------------------------------------------------------------------------
        if (LevelManager.reachedBoss && Boss.BossDefeated == false)// If boss is reached
        {
            // level 01
            if (SceneManager.GetActiveScene().name == "Final")
            {
                bossScreen = new Vector3(142.2765f, 0.6610f, -10f);
                currentPos = transform.position;
            }
            // level 02
            else if (SceneManager.GetActiveScene().name == "Level02") 
            {
                bossScreen = new Vector3(104.354f, 0.645992f, -10f);
                currentPos = transform.position;
            }

            // Moves the camera to boss camera
            transform.position = Vector3.MoveTowards(currentPos, bossScreen, 10f * Time.fixedDeltaTime);
        }
        else
        {
            if (p1 != null)
            {
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
                    if (p1.transform.position.x > canMoveCamera)
                    {

                        transform.position += camDirection * feedBackLoop;
                    }
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------
        if (p1 != null)
        {
            if (p1.transform.position.x > canMoveCamera)
            {
                cameraMoving = true;
            }
        }
    }

    // Creates a rectangle
    Rect CreateRect()
    {
        Rect rect = cameraTrap;
        rect.position += new Vector2(-rect.width / 2, -rect.height / 2);
        // Camera's x and y
        rect.position += new Vector2(transform.position.x, transform.position.y);

        return rect;
    }

    // Prints a rectangle on editor
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
        Gizmos.DrawLine(new Vector3(canMoveCamera, -30f, 0f), new Vector3(canMoveCamera, 30f, 0f));
        Gizmos.DrawLine(new Vector3(maxLevelRangeXmax, -30f, 0f), new Vector3(maxLevelRangeXmax, 30f, 0f));
    }
}