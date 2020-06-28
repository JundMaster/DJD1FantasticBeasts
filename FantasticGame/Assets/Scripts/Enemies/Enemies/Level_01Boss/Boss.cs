using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Boss : Human
{
    // Timers for box spawn + box spawn
    [SerializeField] private GameObject boxSpawn;
    [SerializeField] private Transform position1;
    [SerializeField] private Transform position2;
    private float spawnCounter;
    private float spawnCounterDelay;

    protected override void Start()
    {
        Stats = new Stats();
        animator = GetComponent<Animator>();
        p1 = FindObjectOfType<PlayerMovement>();

        // General
        Stats.IsAlive = true;
        startingPos = transform.position;
        Stats.CurrentHP = HP;
        Stats.MaxHP     = HP;

        // Attack Delay
        Stats.CanRangeAttack = false;
        Stats.RangedAttackDelay = attackDelay;

        // Public Getters
        Damage = enemyDamage;
        PushForce = attackPushForce;

        // Position and Movement
        waitingTimeCounter = 0.5f;
        originalSpeed = speed;
        canMoveTimer = 0;

        spawnCounterDelay = 15;
        spawnCounter = 1;
    }

    protected override void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<PlayerMovement>();
        }

        // ANIMATIONS
        Animations();
        animator.SetBool("shootAnimation", shootAnimation);

        // BOX SPAWN
        SpawnBox();

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // Checks if the enemy dies
        Die();
    }

    private void SpawnBox()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter < 0)
        {
            Instantiate(boxSpawn, position1.position, position1.rotation);
            Instantiate(boxSpawn, position2.position, position2.rotation);
            spawnCounter = spawnCounterDelay;
        }
    }

    protected override void AimCheck()
    {
        if (p1 != null)
        {
            RaycastHit2D aimTop = Physics2D.Raycast(attackPosition.position, attackPosition.right, maxAimRange, playerLayer);
            RaycastHit2D aimJump = Physics2D.Raycast(magicJumpPosition.position, attackPosition.right, maxAimRange, playerLayer);
            RaycastHit2D aimCrouch = Physics2D.Raycast(magicCrouchPosition.position, attackPosition.right, maxAimRange, playerLayer);
            if (aimTop.rigidbody == p1.Rb || aimJump.rigidbody == p1.Rb || aimCrouch.rigidbody == p1.Rb)
            {
                shootAnimation = false; // FOR ANIMATOR
                attacking = true; // FOR ANIMATOR

                // Sets a timer to move, sets a timer to attack
                canMoveTimer = attackDelay;
                Stats.RangedAttackDelay -= Time.deltaTime;

                if (Stats.RangedAttackDelay < 0)
                {
                    shootAnimation = true; // FOR ANIMATOR // CALLS SHOOT()
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

    protected override void Die()
    {
        // ALIVE --------------------------------------------------------------------------------------
        if (!(Stats.IsAlive))
        {
            // Considers boss as defeated
            LevelManager.BossDefeated = true;

            // Destroys boxes
            GameObject[] bossBoxSpawn = GameObject.FindGameObjectsWithTag("BossBoxSpawn");
            foreach (GameObject box in bossBoxSpawn)
            {
                Destroy(box);
            }
           
            Instantiate(deathSpawn, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);

            Destroy(gameObject);
        }
    }
}
