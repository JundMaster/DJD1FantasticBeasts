using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats { get; private set; }

    [SerializeField] Transform          magicPosition;
    [SerializeField] GameObject         magicPrefab;


    [SerializeField] LayerMask          playerLayer;
    [SerializeField] LayerMask          groundLayer;
    [SerializeField] LayerMask          boxesAndwalls;

    // Drops
    [SerializeField] GameObject         healthPickUp, manaPickUp;
    [SerializeField] Transform          groundRangeCheck, groundCheck, wallCheck, backStab;

    [SerializeField] LineRenderer       aimDraw; //Drawing range


    [SerializeField] Player             player;
  


    [SerializeField] float  speed;       // WALKING SPEED
    [SerializeField] float  limitRange;  // RANGE FOR WALKING
    [SerializeField] float  maxAimRange; // RANGE FOR SHOOTING
    [SerializeField] float  attackDelay; // ATTACK DELAY
    [SerializeField] float  HP;          // CURRENT HP
    [SerializeField] int    lootChance;    // LOOT CHANCE 1 - 10
    [SerializeField] bool   holdPosition;   // HOLDS ENEMY POS
    [SerializeField] bool   staticEnemy;    // IF ENEMY IS A STATIC ENEMY
    [SerializeField] bool   shooter;

    Vector2         startingPos;
    
    bool            limitWalkingRangeReached;
    Vector2         tempPosition;
    float           waitingTimeCounter;
    bool   shooting;


    bool            backStabCheckerEnabled;


    


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

        //waitingTimeCounter = waitingTime;
        waitingTimeCounter = Random.Range(0f, 4f);

        backStabCheckerEnabled = false;

    }

    private void Update()
    {
        // Movement -----------------------------------------------------------------------------------
        if (shooting == true)
        {
            if (staticEnemy == false) holdPosition = true;  // ONLY FOR MOVING ENEMIES
            if (stats.CanRangeAttack) Shoot();
        }
        if (shooting == false && staticEnemy == false) Movement();


        // DRAW MAX RANGE OF ATTACK
        aimDraw.enabled = true;
        aimDraw.SetPosition(0, magicPosition.position);
        if (transform.right.x > 0) aimDraw.SetPosition(1, magicPosition.position + magicPosition.right * maxAimRange);
        if (transform.right.x < 0) aimDraw.SetPosition(1, magicPosition.position + new Vector3(-maxAimRange, 0f, 0f));

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // SHOOT ---------------------------------------------------------------------------------------
        if (stats.CanRangeAttack == false)
            stats.RangedAttackCounter -= Time.deltaTime;
        if (stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            stats.RangedAttackCounter = stats.RangedAttackDelay;
            stats.CanRangeAttack = true;
        }

        // CHECKS IF PLAYER IS ON THE ENEMY'S BACK ----------------------------------------------------
        BackStabCheck();

        if (shooter) Shooter();


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

    
    // Checks if the player is behind the enemy, if this is true, rotates the enemy
    void BackStabCheck()
    {
        if (backStabCheckerEnabled)
        {
            Collider2D checkSurround = Physics2D.OverlapCircle(backStab.position, 0.3f, playerLayer);

            if (checkSurround)
            {
                transform.Rotate(0f, 180f, 0f);
                if (limitWalkingRangeReached == true) limitWalkingRangeReached = false;
            }
        }
    }
    

    // Checks if the the player is in range and if there's an object between the enemy and player
    void AimCheck()
    {

        RaycastHit2D aim = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange);
        if (aim)
        {
            if (aim.rigidbody == player.movement.rb)
                shooting = true;
            else
            {
                shooting = false;
                holdPosition = false;
            }
        }
    }

    // Shoots
    void Shoot()
    {
        backStabCheckerEnabled = true; // first time the enemy shoots, it enabled the backstabchecker
        stats.CanRangeAttack = false;
        Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
    }

    // Movement, turns 180 if reaches max position || if collides against a wall || if doesn't detect ground
    // Uses a random timer to turn the enemy
    void Movement()
    {
        Collider2D isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);
        if (isGroundedCheck && holdPosition == false)
        {
            transform.position += transform.right * speed * Time.deltaTime;

            // FRONT WALLS
            Collider2D frontWall = Physics2D.OverlapCircle(wallCheck.position, 0.02f, boxesAndwalls);
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

    void Shooter()
    {
        if (stats.CanRangeAttack == false)
            stats.RangedAttackCounter -= Time.deltaTime;
        if (stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            stats.RangedAttackCounter = stats.RangedAttackDelay;
            stats.CanRangeAttack = true;
        }

        if (stats.CanRangeAttack)
        {
            Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
            stats.CanRangeAttack = false;
        }
    }
}





