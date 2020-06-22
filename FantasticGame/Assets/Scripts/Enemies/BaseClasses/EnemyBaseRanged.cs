using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseRanged : EnemyBase
{
    // Editor stuff
    [SerializeField] protected GameObject magicPrefab;
    [SerializeField] protected Transform  magicJumpPosition;
    [SerializeField] protected float  maxAimRange; // RANGE FOR SHOOTING
    [SerializeField] protected bool   staticEnemy;    // IF ENEMY IS A STATIC ENEMY

    // Public Getters
    public float Damage { get; protected set; }
    public float PushForce { get; protected set; }

    // Attack
    protected bool shootAnimation;


    protected virtual void Start()
    {
        Stats       = new Stats();
        animator    = GetComponent<Animator>();
        p1          = FindObjectOfType<Player>();

        // General
        Stats.IsAlive   = true;
        startingPos     = transform.position;
        Stats.CurrentHP = HP;

        // Attack Delay
        Stats.CanRangeAttack    = false;
        Stats.RangedAttackDelay = attackDelay;

        // Public Getters
        Damage = enemyDamage;
        PushForce = attackPushForce;

        // Position and Movement
        limitWalkingRangeReached = false;
        waitingTimeCounter      = Random.Range(1f, 3f);
        originalSpeed           = speed;
        canMoveTimer            = 0;
    }


    protected virtual void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<Player>();
        }

        if (staticEnemy)
        {
            speed = 0;
            limitWalkingRangeReached = true;
        }

        // ANIMATIONS
        Animations();
        animator.SetBool("shootAnimation", shootAnimation);

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // Checks if the enemy dies
        base.Die();
    }


    // Checks if the the player is in range and if there's an object between the enemy and player
    protected virtual void AimCheck()
    {
        if (p1 != null)
        {
            RaycastHit2D aimTop = Physics2D.Raycast(attackPosition.position, attackPosition.right, maxAimRange);
            RaycastHit2D aimJump = Physics2D.Raycast(magicJumpPosition.position, attackPosition.right, maxAimRange);
            if (aimTop.rigidbody == p1.Movement.Rb || aimJump.rigidbody == p1.Movement.Rb)
            {
                shootAnimation = false; // FOR ANIMATOR
                attacking = true; // FOR ANIMATOR

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
                    attacking = false; // FOR ANIMATOR
                    if (staticEnemy == false) Movement();
                }
            }
        }        
    }

    // Shoots
    protected void Shoot()
    {
        SoundManager.PlaySound(AudioClips.magicAttack); // Plays sound

        shootAnimation = true; // FOR ANIMATOR

        GameObject projectileObject = Instantiate(magicPrefab, attackPosition.position, attackPosition.rotation);
        EnemyAmmunition ammo = projectileObject.GetComponent<EnemyAmmunition>();

        ammo.enemy = this;
    }

    // Movement, turns 180 if reaches max position || if collides against a wall || if doesn't detect ground
    // Uses a random timer to turn the enemy
    protected void Movement()
    {
        if (attacking == false)
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
                    waitingTimeCounter = Random.Range(1f, 3f);
                }
            }
        }
    }
}
