using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMelee : EnemyBase
{
    // Editor stuff
    [SerializeField] private LayerMask  playerLayer;
    [SerializeField] private Transform  backStab;
    [SerializeField] private GameObject ammunitionHit;

    // Attack
    private float       nextAttacksDelay;   // AFTER FIRST ATTACK
    private Collider2D  atackingCollider;

    // Movement
    private Vector2     tempPosition;

    private void Start()
    {
        animator    = GetComponent<Animator>();
        p1          = FindObjectOfType<Player>();

        Stats = new Stats
        {
            IsAlive         = true,
            CurrentHP       = HP,

            MeleeDamage     = enemyDamage,
            CanMeleeAttack  = false,
            MeleeAttackDelay = attackDelay
        };

        // initial position and movement timers
        startingPos     = transform.position;
        originalSpeed   = speed;

        limitWalkingRangeReached    = false;
        waitingTimeCounter          = Random.Range(1f, 3f);

        // attack
        attackDelay         = 0.75f;
        nextAttacksDelay    = 0.99f;
        attacking           = false;
    }

    private void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<Player>();
        }

        // OTHER ATTACKS ANIMATION DELAY
        if (attacking == true)
        {
            Stats.MeleeAttackDelay -= Time.deltaTime;
        }
        if (Stats.MeleeAttackDelay < 0)
        {   // If timeDelay gets < 0, sets timer back to initial delay again and the character can attack
            Melee();
            Stats.MeleeAttackDelay = nextAttacksDelay;
        }
        if (attacking == false)
            Movement();

        //  BACKSTAB ----------------------------------------------------------------------------------
        BackStabCheck();

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // Checks if the enemy dies
        base.Die();

        // ANIMATIONS --------------------------------------------------------------------------------
        Animations();
        animator.SetFloat("canMoveTimer", canMoveTimer);
    }

    // Checks if the player is on the enemy's back
    void BackStabCheck()
    {
        Collider2D checkSurround = Physics2D.OverlapCircle(backStab.position, 0.13f, playerLayer);

        if (checkSurround && attacking == false)
        {
            transform.Rotate(0f, 180f, 0f);
            if (limitWalkingRangeReached == true) limitWalkingRangeReached = false;
        }
    }

    // Checks if the the player is in range and if there's an object between the enemy and player
    void AimCheck()
    {
        atackingCollider = Physics2D.OverlapBox(attackPosition.position, new Vector3(0.4f, 0.7f, 0f) , 0f, playerLayer);

        if (atackingCollider != null)
        {
            attacking = true;
            speed = 0;
            // Sets canMoveTimer to attackDelay, to start counting in the beggining of the attack -- Check else condition --
            canMoveTimer = attackDelay;
        }
        else
        {
            // ONLY STARTS MOVING AGAIN ONCE THE CANMOVETIMER IS < 0
            canMoveTimer -= Time.deltaTime;
            if (canMoveTimer < 0)
            {
                // If player moves out of range, resets animation counter to initial value
                Stats.MeleeAttackDelay = attackDelay;

                if (limitWalkingRangeReached == false)          // If it's maximum range
                {
                    speed = originalSpeed;
                    attacking = false;
                }
              
                if (limitWalkingRangeReached && attacking)    // If it's maximum range and has not collider to attack
                {
                    speed = originalSpeed;
                    attacking = false;
                }

                if (limitWalkingRangeReached)   // If the player leaves its reach while his ranged reached is true
                {
                    waitingTimeCounter -= Time.deltaTime;
                    transform.position = tempPosition;
                }

                if (p1 == null) // If it killed the player
                {
                    speed = originalSpeed;
                    attacking = false;
                    limitWalkingRangeReached = false;
                }
            }         
        }
    }

    // Melee Attack
    void Melee()
    {
        SoundManager.PlaySound(AudioClips.magicAttack); // Plays sound

        // Delayed Attack Damage
        attacking = false;
        if (atackingCollider != null)
        {
            p1.Stats.TakeDamage(Stats.MeleeDamage);
            if (p1.Movement.CrouchGetter)
            {
                Instantiate(ammunitionHit, p1.transform.position + new Vector3(0f, 0.3f, 0f), p1.transform.rotation);
            }
            else
            {
                Instantiate(ammunitionHit, p1.transform.position + new Vector3(0f, 0.5f, 0f), p1.transform.rotation);
            }

            // Pushes the player
            if (p1.transform.position.x > transform.position.x)
            {
                p1.Movement.Rb.AddForce(new Vector2(attackPushForce, 0f));
            }
            else if (p1.transform.position.x < transform.position.x)
            {
                p1.Movement.Rb.AddForce(new Vector2(-attackPushForce, 0f));
            }

            // Shakes the screen
            StartCoroutine(p1.CameraShake.Shake(0.025f, 0.08f));
        }
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

            // NO FLOOR
            Collider2D goundRangeCheck = Physics2D.OverlapCircle(groundRangeCheck.position, 0.1f, groundLayer);
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
            if (limitWalkingRangeReached && attacking == false)
            {
                waitingTimeCounter -= Time.deltaTime;
                transform.position = tempPosition;
            }
            if (waitingTimeCounter < 0)
            {
                transform.Rotate(0f, 180f, 0f);
                waitingTimeCounter = Random.Range(1f, 4f); // WAITING TIME <<<<<<<<<<< TA RANDOM NESTE ;
                limitWalkingRangeReached = false;
            }
        }
    }

    // prints enemy attack range on editor
    private void OnDrawGizmos()
    {
        if (transform.right.x > 0) Gizmos.DrawWireCube(attackPosition.position, new Vector3( 0.4f, 0.7f, 0f));
        if (transform.right.x < 0) Gizmos.DrawWireCube(attackPosition.position, new Vector3( -0.4f, 0.7f, 0f));
        Gizmos.DrawWireSphere(backStab.position, 0.13f);
    }
}





