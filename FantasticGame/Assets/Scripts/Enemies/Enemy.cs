using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats { get; private set; }

    [SerializeField] Transform      magicPosition;
    [SerializeField] GameObject     magicPrefab;


    [SerializeField] LayerMask      playerLayer;

    // Drops
    [SerializeField] GameObject     healthPickUp, manaPickUp;



    float           speed;

    Vector2         startingPos;
    float           limitRange;
    bool            limitRangedReached;
    Vector2         tempPosition;

    float           waitingTimeCounter;
    float           waitingTime;


    //Collider2D      aimRange;
    RaycastHit2D    aimRange;
    float           maxAimRange;
    bool            isShooting;


    private void Awake()
    {
        stats = new Stats();
    }

    private void Start()
    {
        stats.CanRangeAttack = false;
        stats.RangedAttackDelay = 2f;

        startingPos = transform.position;

        stats.CurrentHP = 100f;
        speed = 0.5f;

        limitRange = 0.5f;
        limitRangedReached = false;
        waitingTime = 2f;
        waitingTimeCounter = waitingTime;

        maxAimRange = 1f;
    }

    private void Update()
    {
        // UPDATE VARIABLES ----------------------------------------------------------------------------
        

        


        // SHOOT ---------------------------------------------------------------------------------------
        if (stats.CanRangeAttack == false)
            stats.RangedAttackCounter -= Time.deltaTime;
        if (stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            stats.RangedAttackCounter = stats.RangedAttackDelay;
            stats.CanRangeAttack = true;
        }

        //aimRange = Physics2D.OverlapCircle(transform.position, maxAimRange, playerLayer);
        aimRange = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange, playerLayer);

        if (aimRange)
        {
            isShooting = true;
            if (stats.CanRangeAttack)
                Shoot();
        }
        else
            isShooting = false;


        // Movement -----------------------------------------------------------------------------------
        if (!(isShooting))
            Movement();


        // ALIVE --------------------------------------------------------------------------------------
        if (!(stats.IsAlive))
        {
            int chance = Random.Range(0, 10);
            if (chance > 6)
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
        transform.position += transform.right * speed * Time.deltaTime;

        if ((transform.position.x > startingPos.x + limitRange || transform.position.x < startingPos.x - limitRange) && limitRangedReached == false)
        {
            limitRangedReached = true;
            tempPosition = transform.position;
        }

        // WAITING TIME DELAY // If it reaches the limit distance, starts walking back
        if (limitRangedReached)
        {
            waitingTimeCounter -= Time.deltaTime;
            transform.position = tempPosition;
        }
        if (waitingTimeCounter < 0)
        {
            transform.Rotate(0f, 180f, 0f);
            waitingTimeCounter = Random.Range(0f, 2f); // WAITING TIME <<<<<<<<<<< TA RANDOM NESTE 
            transform.position += transform.right * speed * Time.deltaTime;
            limitRangedReached = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, maxAimRange);
        Gizmos.DrawLine(magicPosition.position, magicPosition.position + new Vector3(maxAimRange,0f,0f));
    }
}
