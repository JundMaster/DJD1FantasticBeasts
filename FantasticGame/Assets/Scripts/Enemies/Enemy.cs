using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats Stats { get; private set; }

    [SerializeField] Transform          magicPosition;
    [SerializeField] Transform          magicPositionCrouch;
    [SerializeField] GameObject         magicPrefab;


    [SerializeField] LayerMask          playerLayer;
    [SerializeField] LayerMask          groundLayer;
    [SerializeField] LayerMask          boxesAndwalls;

    // Drops
    [SerializeField] GameObject         healthPickUp, manaPickUp;
    [SerializeField] Transform          groundRangeCheck, groundCheck, wallCheck, backStab;

    //[SerializeField] LineRenderer       aimDraw; //Drawing range
  

    [SerializeField] float  speed;       // WALKING SPEED
    [SerializeField] float  limitRange;  // RANGE FOR WALKING
    [SerializeField] float  maxAimRange; // RANGE FOR SHOOTING
    [SerializeField] float  attackDelay; // ATTACK DELAY
    [SerializeField] float  HP;          // CURRENT HP

    [SerializeField] float  enemyDamage;     // ENEMY DAMAGE
    [SerializeField] int    lootChance;    // LOOT CHANCE 1 - 10
    private bool            holdPosition;   // HOLDS ENEMY POS
    [SerializeField] bool   staticEnemy;    // IF ENEMY IS A STATIC ENEMY

    //[SerializeField] bool   shooter; // ONLY FOR DEMO

    public float    Damage { get; private set; }
    bool            shooting;

    float           originalSpeed;
    Vector2         startingPos;
    bool            limitWalkingRangeReached;
    Vector2         tempPosition;
    float           waitingTimeCounter;
    bool            backStabCheckerEnabled;
    

    private void Awake()
    {
        Stats = new Stats();
    }

    private void Start()
    {
        Stats.IsAlive = true;

        Stats.CanRangeAttack = false;
        Stats.RangedAttackDelay = attackDelay;

        startingPos = transform.position;
        Stats.CurrentHP = HP;

        Stats.RangedDamage = enemyDamage;
        Stats.MeleeDamage = enemyDamage;
        Damage = enemyDamage;

        limitWalkingRangeReached = false;

        //waitingTimeCounter = waitingTime;
        waitingTimeCounter = Random.Range(1f, 3f);

        backStabCheckerEnabled = false;

        originalSpeed = speed;

    }

    private void Update()
    {
        // Movement -----------------------------------------------------------------------------------
        if (shooting == true)
        {
            if (staticEnemy == false) holdPosition = true;  // ONLY FOR MOVING ENEMIES
            if (Stats.CanRangeAttack) Shoot();
        }
        if (shooting == false && staticEnemy == false) Movement();


        // DRAW MAX RANGE OF ATTACK
        //aimDraw.enabled = true;
        //aimDraw.SetPosition(0, magicPosition.position);
        //if (transform.right.x > 0) aimDraw.SetPosition(1, magicPosition.position + new Vector3(maxAimRange, 0f, -1f));
        //if (transform.right.x < 0) aimDraw.SetPosition(1, magicPosition.position + new Vector3(-maxAimRange, 0f, -1f));

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // SHOOT ---------------------------------------------------------------------------------------
        if (Stats.CanRangeAttack == false)
            Stats.RangedAttackCounter -= Time.deltaTime;
        if (Stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            Stats.RangedAttackCounter = Stats.RangedAttackDelay;
            Stats.CanRangeAttack = true;
        }

        // CHECKS IF PLAYER IS ON THE ENEMY'S BACK ----------------------------------------------------
        BackStabCheck();

        // ONLY FOR DEMO
        // if (shooter) Shooter();


        // ALIVE --------------------------------------------------------------------------------------
        if (!(Stats.IsAlive))
        {
            int chance = Random.Range(0, 10);
            if (chance > lootChance)
            {
                if (healthPickUp != null && chance >= 5) Instantiate(healthPickUp, transform.position, transform.rotation);
                else if (manaPickUp != null) Instantiate(manaPickUp, transform.position, transform.rotation);
            }
               
            Stats.Die(gameObject);
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
        Player p1 = FindObjectOfType<Player>();
        RaycastHit2D aimTop = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange);
        RaycastHit2D aimBottom = Physics2D.Raycast(magicPositionCrouch.position, magicPositionCrouch.right, maxAimRange);
        if (aimTop.rigidbody == p1.Movement.Rb || aimBottom.rigidbody == p1.Movement.Rb)
        {
            shooting = true;
            speed = 0;
        }
        else
        {
            shooting = false;
            holdPosition = false;
        }

        if (transform.right.x > 0)
            if (p1.transform.position.x > magicPosition.position.x + maxAimRange)
                speed = originalSpeed;

        if (transform.right.x < 0)
            if (p1.transform.position.x < magicPosition.position.x - maxAimRange)
                speed = originalSpeed;
    }

    // Shoots
    void Shoot()
    {
        backStabCheckerEnabled = true; // first time the enemy shoots, it enabled the backstabchecker
        Stats.CanRangeAttack = false;
        GameObject projectileObject = Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
        EnemyAmmunition ammo = projectileObject.GetComponent<EnemyAmmunition>();

        ammo.enemy = this;
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

    private void OnDrawGizmos()
    {
        if (transform.right.x >= 0) Gizmos.DrawWireSphere(magicPosition.position + new Vector3(maxAimRange, 0f, -1f), 0.05f);
        if (transform.right.x < 0) Gizmos.DrawWireSphere(magicPosition.position + new Vector3(-maxAimRange, 0f, -1f), 0.05f);
    }


    /*  ONLY FOR DEMO
    void Shooter()
    {
        if (Stats.CanRangeAttack == false)
            Stats.RangedAttackCounter -= Time.deltaTime;
        if (Stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            Stats.RangedAttackCounter = Stats.RangedAttackDelay;
            Stats.CanRangeAttack = true;
        }

        if (Stats.CanRangeAttack)
        {
            Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
            Stats.CanRangeAttack = false;
        }
    }
    */
}





