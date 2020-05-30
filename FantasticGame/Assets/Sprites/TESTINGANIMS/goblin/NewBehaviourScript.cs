﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour

{
    public Stats Stats { get; private set; }

    [SerializeField] Transform magicPosition;
    [SerializeField] Transform magicPositionCrouch;
    [SerializeField] GameObject magicPrefab;


    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask boxesAndwalls;

    [SerializeField] Transform groundRangeCheck, groundCheck, wallCheck, backStab;

    // Drops
    [SerializeField] GameObject healthPickUp, manaPickUp;

    [SerializeField] float speed;       // WALKING SPEED
    [SerializeField] float limitRange;  // RANGE FOR WALKING
    [SerializeField] float maxAimRange; // RANGE FOR SHOOTING
    [SerializeField] float backStabSize; // BACK STAB COLLIDER SIZE
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
    Vector2 tempPosition;
    float waitingTimeCounter;
    bool backStabCheckerEnabled;

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


        backStabCheckerEnabled = false;
        originalSpeed = speed;
        canMoveTimer = attackDelay;
    }

    private void Update()
    {
        animator.SetFloat("speed", speed);
        animator.SetBool("limitReached", limitWalkingRangeReached);
        animator.SetBool("shooting", shooting);
        animator.SetBool("shootAnimation", shootAnimation);



        // ATTACK DELAY -------------------------------------------------------------------------------
        if (Stats.CanRangeAttack == false)
        {
            Stats.RangedAttackCounter -= Time.deltaTime;
            shootAnimation = true;              // Sets animation to true
        }

        if (Stats.RangedAttackCounter < 0.7f)   // Sets animation false after 0.3f
            shootAnimation = false;

        if (Stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            Stats.RangedAttackCounter = Stats.RangedAttackDelay;
            Stats.CanRangeAttack = true;
        }

        // Movement -----------------------------------------------------------------------------------
        // Attack delays for ranged attacks
        if (shooting == true) // RANGED
        {
            if (Stats.CanRangeAttack)
            {
                Shoot();
            }
        }
        if (shooting == false && staticEnemy == false) Movement();

        
        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();


        Debug.Log(shootAnimation);

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
            Collider2D checkSurround = Physics2D.OverlapCircle(backStab.position, 0.5f, playerLayer);

            if (checkSurround)
            {
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }


    // Checks if the the player is in range and if there's an object between the enemy and player
    void AimCheck()
    {
        Player p1 = FindObjectOfType<Player>();
        RaycastHit2D aimTop = Physics2D.Raycast(magicPosition.position, magicPosition.right, maxAimRange);
        if (aimTop.rigidbody == p1.Movement.Rb)
        {
            shooting = true;
            speed = 0;
            canMoveTimer = attackDelay;
        }
        else
        {
            shooting        = false;
            canMoveTimer    -= Time.deltaTime;


            if (canMoveTimer < 0.5f)
            {

                if (limitWalkingRangeReached)
                {
                    waitingTimeCounter -= Time.deltaTime;
                }

                if (limitWalkingRangeReached == false)
                {
                    speed = originalSpeed;
                }
            }
        }




    }

    // Shoots
    void Shoot()
    {
        backStabCheckerEnabled = false; // first time the enemy shoots, it enabled the backstabchecker
        Stats.CanRangeAttack = false;
        GameObject projectileObject = Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
        EnemyAmmunition ammo = projectileObject.GetComponent<EnemyAmmunition>();

        // falta meter o dano
    }

    // Movement, turns 180 if reaches max position || if collides against a wall || if doesn't detect ground
    // Uses a random timer to turn the enemy
    void Movement()
    {
        Collider2D isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);
        if (isGroundedCheck)
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
                speed = 0;
            }
            if (waitingTimeCounter < 0)
            {
                transform.Rotate(0f, 180f, 0f);
                waitingTimeCounter = Random.Range(0f, 4f); // WAITING TIME <<<<<<<<<<< TA RANDOM NESTE ;
                transform.position += transform.right * speed * Time.deltaTime;
                limitWalkingRangeReached = false;
                speed = originalSpeed;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (transform.right.x >= 0) Gizmos.DrawWireSphere(magicPosition.position + new Vector3(maxAimRange, 0f, -1f), 0.05f);
        if (transform.right.x < 0) Gizmos.DrawWireSphere(magicPosition.position + new Vector3(-maxAimRange, 0f, -1f), 0.05f);
        Gizmos.DrawWireSphere(backStab.position, backStabSize);
    }
}




