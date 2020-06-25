using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Stats Stats { get; protected set; }

    // Editor variables
    [SerializeField] protected LayerMask    groundLayer;
    [SerializeField] protected LayerMask    boxesAndwalls;
    [SerializeField] protected LayerMask    playerLayer;
    [SerializeField] protected Transform    backStab;
    [SerializeField] protected Transform    attackPosition;
    [SerializeField] protected GameObject   healthPickUp, manaPickUp, deathSpawn;
    [SerializeField] protected Transform    groundRangeCheck, groundCheck, wallCheck;
    [SerializeField] protected RectTransform healthBarRect;

    [SerializeField] protected float    HP;          // CURRENT HP
    [SerializeField] protected float    enemyDamage;     // ENEMY DAMAGE
    [SerializeField] protected float    speed;              // WALKING SPEED
    [SerializeField] protected float    attackDelay; // ATTACK DELAY
    [SerializeField] protected float    limitRange;         // RANGE FOR WALKING
    [SerializeField] protected float    attackPushForce;    // HOW MUCH WILL ENEMY PUSH THE PLAYER
    [SerializeField] protected int      lootChance;         // LOOT CHANCE 1 - 10

    // Destroyable object after death
    [SerializeField] protected GameObject destroyObject;

    // Player
    protected PlayerMovement p1;

    // Attack
    protected bool attacking;
    
    // Movement
    protected float     originalSpeed;
    protected Vector2   startingPos;
    protected float     canMoveTimer;
    protected float     waitingTimeCounter;
    // Max distances
    protected bool      leftDistReached     = false;
    protected bool      rightDistReached    = false;


    // Animator
    protected Animator animator;
    protected void Animations()
    {
        animator.SetFloat("speed", speed);
        animator.SetBool("attacking", attacking);
    }


    // Movement, turns 180 if reaches max position || if collides against a wall || if doesn't detect ground
    // Uses a random timer to turn the enemy
    protected virtual void Movement()
    {
        if (attacking == false)
        {
            Collider2D isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
            if (isGroundedCheck)
            {
                transform.position += transform.right * speed * Time.deltaTime;

                // Checks walls, floors, or min and max range
                MovementLimitsCheck();

                // WAITING TIME DELAY // If it reaches the limit distance, starts walking back
                if (leftDistReached || rightDistReached)
                {
                    waitingTimeCounter -= Time.deltaTime;
                    speed = 0;
                }
                if (waitingTimeCounter < 0)
                {
                    // Sets max distances to false
                    leftDistReached = false;
                    rightDistReached = false;

                    // Rotates the enemy and sets its original speed
                    transform.Rotate(0, 180f, 0f);
                    speed = originalSpeed;

                    // Gives a new waiting timer for the next time it reaches max position
                    waitingTimeCounter = Random.Range(1f, 3f);
                }
            }
        }
    }

    // Checks walls, floors, or min and max range
    protected void MovementLimitsCheck()
    {
        // FRONT WALLS
        Collider2D frontWall = Physics2D.OverlapCircle(wallCheck.position, 0.02f, boxesAndwalls);
        if (frontWall != null && transform.right.x < 0)
        {   // LEFT
            leftDistReached = true;
        }
        else if (frontWall != null && transform.right.x > 0)
        {   // RIGHT
            rightDistReached = true;
        }

        Collider2D goundRangeCheck = Physics2D.OverlapCircle(groundRangeCheck.position, 0.1f, groundLayer);
        // NO FLOOR
        if (goundRangeCheck == null && transform.right.x < 0)
        {   // LEFT
            leftDistReached = true;
        }
        else if (goundRangeCheck == null && transform.right.x > 0)
        {   // RIGHT
            rightDistReached = true;
        }

        // MAX RANGE
        if (transform.position.x > startingPos.x + limitRange && transform.right.x > 0)
        {   // RIGHT
            rightDistReached = true;
        }
        else if (transform.position.x < startingPos.x - limitRange && transform.right.x < 0)
        {   // LEFT
            leftDistReached = true;
        }
    }

    // Checks if the player is on the enemy's back
    protected virtual void BackStabCheck()
    {
        Collider2D checkSurround = Physics2D.OverlapCircle(backStab.position, 0.15f, playerLayer);

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

    protected virtual void Die()
    {
        // ALIVE --------------------------------------------------------------------------------------
        if (!(Stats.IsAlive))
        {
            if (destroyObject)
            {
                Destroy(destroyObject);
            }

            Instantiate(deathSpawn, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);

            int chance = Random.Range(0, 10);
            if (chance > lootChance)
            {
                if (healthPickUp != null && chance >= 5)
                {
                    Instantiate(healthPickUp, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);
                }
                else if (manaPickUp != null)
                {
                    Instantiate(manaPickUp, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);
                }
            }
            Destroy(gameObject);
        }
    }
}





