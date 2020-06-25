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

    protected virtual void Awake()
    {
        p1 = FindObjectOfType<PlayerMovement>();
    }

    protected virtual void Start()
    {
        Stats       = new Stats();
        animator    = GetComponent<Animator>();

        // General
        Stats.IsAlive   = true;
        startingPos     = transform.position;
        Stats.CurrentHP = HP;
        Stats.MaxHP     = HP;

        // Attack Delay
        Stats.CanRangeAttack    = false;
        Stats.RangedAttackDelay = attackDelay;

        // Public Getters
        Damage = enemyDamage;
        PushForce = attackPushForce;

        // Position and Movement
        waitingTimeCounter      = Random.Range(1f, 3f);
        originalSpeed           = speed;
        canMoveTimer            = 0;
    }


    protected virtual void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<PlayerMovement>();
        }

        if (staticEnemy)
        {
            speed = 0;
        }

        if (healthBarRect)
        {
            healthBarRect.localScale = new Vector2(Stats.CurrentHP / Stats.MaxHP, 1f);
        }

        //  BACKSTAB ----------------------------------------------------------------------------------
        BackStabCheck();

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // Checks if the enemy dies
        base.Die();

        // ANIMATIONS
        Animations();
        animator.SetBool("shootAnimation", shootAnimation);
    }


    // Checks if the the player is in range and if there's an object between the enemy and player
    protected virtual void AimCheck()
    {
        if (p1 != null)
        {
            RaycastHit2D aimTop = Physics2D.Raycast(attackPosition.position, attackPosition.right, maxAimRange);
            RaycastHit2D aimJump = Physics2D.Raycast(magicJumpPosition.position, attackPosition.right, maxAimRange);
            if (aimTop.rigidbody == p1.Rb || aimJump.rigidbody == p1.Rb)
            {
                shootAnimation = false; // FOR ANIMATOR
                attacking = true; // FOR ANIMATOR

                // Sets a timer to move, sets a timer to attack
                canMoveTimer = attackDelay;
                Stats.RangedAttackDelay -= Time.deltaTime;

                if (Stats.RangedAttackDelay < 0)
                {
                    shootAnimation = true; // FOR ANIMATOR // CALLS SHOOT
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

    // Shoots // CALLED WHEN THE ENEMIES SHOOT IN THE ANIMATION // ANIMATION EVENT
    protected virtual void Shoot()
    {
        SoundManager.PlaySound(AudioClips.magicAttack); // Plays sound

        GameObject projectileObject = Instantiate(magicPrefab, attackPosition.position, attackPosition.rotation);
        EnemyAmmunition ammo = projectileObject.GetComponent<EnemyAmmunition>();

        ammo.enemy = this;
    }

    // Checks if the player is on the enemy's back
    protected override void BackStabCheck()
    {
        Collider2D checkSurround = Physics2D.OverlapCircle(backStab.position, 0.32f, playerLayer);

        if (checkSurround)
        {
            // Turns attack off
            attacking = false;

            // Sets max distances to false
            leftDistReached = false;
            rightDistReached = false;

            // Rotates the enemy and sets its original speed
            transform.Rotate(0, 180f, 0f);
            speed = originalSpeed;

            canMoveTimer = attackDelay * 2f;

            // Gives a new waiting timer for the next time it reaches max position
            waitingTimeCounter = Random.Range(1f, 3f);
        }
    }

    private void OnDrawGizmos()
    {
        if (backStab) Gizmos.DrawWireSphere(backStab.position, 0.32f);
    }
}
