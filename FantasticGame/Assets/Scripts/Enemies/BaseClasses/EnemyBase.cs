using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Stats Stats { get; protected set; }

    // Editor variables
    [SerializeField] protected LayerMask    groundLayer;
    [SerializeField] protected LayerMask    boxesAndwalls;
    [SerializeField] protected Transform    attackPosition;
    [SerializeField] protected GameObject   healthPickUp, manaPickUp, deathSpawn;
    [SerializeField] protected Transform    groundRangeCheck, groundCheck, wallCheck;

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
    protected Player p1;

    // Attack
    protected bool attacking;
    
    // Movement
    protected float     originalSpeed;
    protected Vector2   startingPos;
    protected bool      limitWalkingRangeReached;
    protected float     canMoveTimer;
    protected float     waitingTimeCounter;

    // Animator
    protected Animator animator;
    protected void Animations()
    {
        animator.SetFloat("speed", speed);
        animator.SetBool("limitReached", limitWalkingRangeReached);
        animator.SetBool("attacking", attacking);
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





