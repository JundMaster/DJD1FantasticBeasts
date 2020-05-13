using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats { get; private set; }

    [SerializeField] Transform      magicPosition;
    [SerializeField] GameObject     magicPrefab;


    [SerializeField] LayerMask      playerLayer;
    [SerializeField] LayerMask      groundLayer;
    [SerializeField] LayerMask      breakRaycast;

    // Drops
    [SerializeField] GameObject     healthPickUp, manaPickUp;
    [SerializeField] Transform      groundRangeCheck, groundCheck, wallCheck;

    [SerializeField] LineRenderer aimDraw; //Drawing range



    [SerializeField] float  speed;       // WALKING SPEED
    [SerializeField] float  limitRange;  // RANGE FOR WALKING
    [SerializeField] float  userInputRange; // RANGE FOR SHOOTING
    [SerializeField] float  attackDelay; // ATTACK DELAY
    [SerializeField] float  HP;          // CURRENT HP
    [SerializeField] int    lootChance;    // LOOT CHANCE 1 - 10
    [SerializeField] bool   holdPosition;

    Vector2         startingPos;
    
    bool            limitWalkingRangeReached;
    Vector2         tempPosition;
    float           waitingTimeCounter;

    float           maxAimRange;
    RaycastHit2D    aimRange;
    bool            isShooting;


    
    private void Awake()
    {
        stats = new Stats();
    }

    private void Start()
    {
        stats.CanRangeAttack = false;
        stats.RangedAttackDelay = attackDelay;

        startingPos = transform.position;
        stats.CurrentHP = HP;

        limitWalkingRangeReached = false;

        maxAimRange = userInputRange;

        //waitingTimeCounter = waitingTime;
        waitingTimeCounter = Random.Range(0f, 4f);
    }

    private void Update()
    {
        // SHOOT ---------------------------------------------------------------------------------------
        if (stats.CanRangeAttack == false)
            stats.RangedAttackCounter -= Time.deltaTime;
        if (stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            stats.RangedAttackCounter = stats.RangedAttackDelay;
            stats.CanRangeAttack = true;
        }

        

        //RaycastHit2D wallRange = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange, groundLayer);
        //if (wallRange)
        //    maxAimRange = 0f;
        //else
        //    maxAimRange = userInputRange;


        aimRange = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange, playerLayer);
        if (aimRange)
        {
            isShooting = true;
            if (stats.CanRangeAttack)
                Shoot();
        }
        else
            isShooting = false;



        // DRAW MAX RANGE OF ATTACK
        aimDraw.enabled = true;
        aimDraw.SetPosition(0, magicPosition.position);
        if (transform.right.x > 0) aimDraw.SetPosition(1, magicPosition.position + new Vector3 (maxAimRange, 0f,0f));
        else aimDraw.SetPosition(1, magicPosition.position - new Vector3(maxAimRange, 0f, 0f));

        // Movement -----------------------------------------------------------------------------------
        if (!(isShooting))
            Movement();


        // ALIVE --------------------------------------------------------------------------------------
        if (!(stats.IsAlive))
        {
            int chance = Random.Range(0, 10);
            if (chance > lootChance)
            {
                if (healthPickUp != null && chance >= 5) Instantiate(healthPickUp, transform.position, transform.rotation);
                else if (manaPickUp != null) Instantiate(manaPickUp, transform.position, transform.rotation);
            }
               
            stats.Die(gameObject);
        }
    }

    void Shoot()
    {
        stats.CanRangeAttack = false;
        Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
    }


    void Movement()
    {
        if (!holdPosition) //static enemmy
        {
            Collider2D isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);
            if (isGroundedCheck)
            {
                transform.position += transform.right * speed * Time.deltaTime;

                // FRONT WALLS
                Collider2D frontWall = Physics2D.OverlapCircle(wallCheck.position, 0.02f, groundLayer);
                if (frontWall != null && limitWalkingRangeReached == false)
                {
                    limitWalkingRangeReached = true;
                    tempPosition = transform.position;
                }

                Collider2D goundRangeCheck = Physics2D.OverlapCircle(groundRangeCheck.position, 0.1f, groundLayer);
                // NO FLOOR
                if (goundRangeCheck == null && limitWalkingRangeReached == false)
                {
                    limitWalkingRangeReached = true;
                    tempPosition = transform.position;
                }

                // MAX RANGE
                if ((transform.position.x > startingPos.x + limitRange || transform.position.x < startingPos.x - limitRange) && limitWalkingRangeReached == false)
                {
                    limitWalkingRangeReached = true;
                    tempPosition = transform.position;
                }

                // WAITING TIME DELAY // If it reaches the limit distance, starts walking back
                if (limitWalkingRangeReached)
                {
                    waitingTimeCounter -= Time.deltaTime;
                    transform.position = tempPosition;
                }
                if (waitingTimeCounter < 0)
                {
                    transform.Rotate(0f, 180f, 0f);
                    waitingTimeCounter = Random.Range(0f, 4f); // WAITING TIME <<<<<<<<<<< TA RANDOM NESTE ;
                    transform.position += transform.right * speed * Time.deltaTime;
                    limitWalkingRangeReached = false;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallCheck.position, 0.02f);
    }

}
