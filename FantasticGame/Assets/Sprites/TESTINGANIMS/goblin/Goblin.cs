using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour

{
    public Stats Stats { get; private set; }

    [SerializeField] Transform magicPosition;
    [SerializeField] GameObject magicPrefab;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask boxesAndwalls;

    [SerializeField] Transform groundRangeCheck, groundCheck, wallCheck;

    // Drops
    [SerializeField] GameObject healthPickUp, manaPickUp;

    [SerializeField] float speed;       // WALKING SPEED
    [SerializeField] float limitRange;  // RANGE FOR WALKING
    [SerializeField] float maxAimRange; // RANGE FOR SHOOTING
    [SerializeField] float attackDelay; // ATTACK DELAY
    [SerializeField] float HP;          // CURRENT HP

    [SerializeField] float enemyDamage;     // ENEMY DAMAGE
    [SerializeField] float attackPushForce;  // HOW MUCH WILL ENEMY PUSH THE PLAYER
    [SerializeField] int lootChance;    // LOOT CHANCE 1 - 10
    [SerializeField] bool staticEnemy;    // IF ENEMY IS A STATIC ENEMY

    //[SerializeField] bool   shooter; // ONLY FOR DEMO

    public float Damage { get; private set; }
    public float PushForce { get; private set; }
    bool shooting;
    bool shootAnimation;


    float originalSpeed;
    Vector2 startingPos;
    bool limitWalkingRangeReached;
    //Vector2 tempPosition;
    float waitingTimeCounter;

    float canMoveTimer;


    Animator animator;
    private void Awake()
    {
        Stats = new Stats();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Stats.IsAlive = true;
        startingPos = transform.position;
        Stats.CurrentHP = HP;

        Stats.CanRangeAttack = false;
        Stats.RangedAttackDelay = attackDelay;

        Stats.RangedDamage = enemyDamage;
        Damage = enemyDamage;
        PushForce = attackPushForce;

        limitWalkingRangeReached = false;
        //waitingTimeCounter = waitingTime;
        waitingTimeCounter = Random.Range(1f, 3f);


        originalSpeed = speed;
        canMoveTimer = 0;
    }

    private void Update()
    {
        if (staticEnemy)
        {
            speed = 0;
            limitWalkingRangeReached = true;
        }

        animator.SetFloat("speed", speed);
        animator.SetBool("limitReached", limitWalkingRangeReached);
        animator.SetBool("shooting", shooting);
        animator.SetBool("shootAnimation", shootAnimation);

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

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


    // Checks if the the player is in range and if there's an object between the enemy and player
    void AimCheck()
    {
        Player p1 = FindObjectOfType<Player>();
        RaycastHit2D aimTop = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange);
        if (aimTop.rigidbody == p1.Movement.Rb)
        {
            shooting = true; // FOR ANIMATOR
            shootAnimation = false; // FOR ANIMATOR
            // Sets a timer to move, sets a timer to attack
            canMoveTimer = attackDelay;
            Stats.RangedAttackDelay -= Time.deltaTime;
            if (Stats.RangedAttackDelay < 0)
            {
                Shoot();
                Stats.RangedAttackDelay = attackDelay;
            }
        }
        else
        {   // If the player leaves max range, if the timer is less than 0, the enemy moves
            canMoveTimer -= Time.deltaTime;
            if (canMoveTimer <= 0)
            {
                shootAnimation = false; // FOR ANIMATOR
                shooting = false; // FOR ANIMATOR
                if (staticEnemy == false) Movement();
            }
        }
    }

    // Shoots
    void Shoot()
    {
        shootAnimation = true; // FOR ANIMATOR
        GameObject projectileObject = Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
        EnemyAmmunition ammo = projectileObject.GetComponent<EnemyAmmunition>();

        ammo.goblinDamage = this;
    }

    // Movement, turns 180 if reaches max position || if collides against a wall || if doesn't detect ground
    // Uses a random timer to turn the enemy
    void Movement()
    {
        if (shooting == false)
        {
            Collider2D isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
            if (isGroundedCheck)
            {
                transform.position += transform.right * speed * Time.deltaTime;

                // FRONT WALLS
                Collider2D frontWall = Physics2D.OverlapCircle(wallCheck.position, 0.02f, boxesAndwalls);
                if (frontWall != null && limitWalkingRangeReached == false)
                {
                    limitWalkingRangeReached = true;
                }

                Collider2D goundRangeCheck = Physics2D.OverlapCircle(groundRangeCheck.position, 0.1f, groundLayer);
                // NO FLOOR
                if (goundRangeCheck == null && limitWalkingRangeReached == false)
                {
                    limitWalkingRangeReached = true;
                }

                // MAX RANGE
                if ((transform.position.x > startingPos.x + limitRange || transform.position.x < startingPos.x - limitRange) && limitWalkingRangeReached == false)
                {
                    limitWalkingRangeReached = true;
                }

                // WAITING TIME DELAY // If it reaches the limit distance, starts walking back
                if (limitWalkingRangeReached)
                {
                    waitingTimeCounter -= Time.deltaTime;
                    speed = 0;
                }
                if (waitingTimeCounter < 0)
                {
                    speed = originalSpeed;
                    transform.Rotate(0, 180f, 0f);
                    limitWalkingRangeReached = false;
                    waitingTimeCounter = 2f; // Random.Range(1f, 3f);
                }
            }
        }
    }
}





