using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

sealed public class Boss_02 : Human
{
    // Timers for box spawn + box spawn
    [SerializeField] private ParticleSystem powerSpawnRight;
    [SerializeField] private ParticleSystem powerSpawnLeft;
    [SerializeField] private Transform position1;
    [SerializeField] private Transform position2;
    [SerializeField] private Transform midPosition;
    [SerializeField] private Transform minRange;
    [SerializeField] private Transform maxRange;


    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject summoningStance;
    [SerializeField] private Collider2D col;

    // Power spawn timers
    private float   spawnCounter;
    private float   spawnCounterDelay;
    private float   usePowerCounter;
    private float   usePowerDelay;
    private bool    canUsePowerAbility;
    private bool    usingPowerAbility;
    private bool    getRandNumbers;

    // Player pos
    private float originalY;

    // Light
    private GameObject  mainScreenLight;
    private Light2D     screenLight;
    private float           innerRadius;
    private readonly float  maxInnerRadius      = 3.4f;
    private readonly float  minInnerRadius      = 0.63f;
    private float           outerRadius;
    private readonly float  maxOuterRadius      = 20.62f;
    private readonly float  minOuterRadius      = 1.2f;

    protected override void Start()
    {
        // Gets components etc
        Stats           = new Stats();
        animator        = GetComponent<Animator>();
        p1              = FindObjectOfType<PlayerMovement>();
        mainScreenLight = GameObject.FindGameObjectWithTag("screenMainLight");
        screenLight     = mainScreenLight.GetComponent<Light2D>();

        // General
        Stats.IsAlive   = true;
        startingPos     = transform.position;
        Stats.CurrentHP = HP;
        Stats.MaxHP     = HP;

        // Attack Delay
        Stats.CanRangeAttack    = false;
        Stats.RangedAttackDelay = attackDelay;

        // Public Getters
        Damage      = enemyDamage;
        PushForce   = attackPushForce;

        // Position and Movement
        waitingTimeCounter  = 0.5f;
        originalSpeed       = speed;
        canMoveTimer        = 0;

        // Power Timers
        spawnCounterDelay   = 10;
        spawnCounter        = 2;
        usePowerDelay       = 6;
        usePowerCounter     = usePowerDelay;

        canUsePowerAbility  = true;
        usingPowerAbility   = false;
        getRandNumbers      = true;

        // Lights
        outerRadius = maxOuterRadius;
        innerRadius = maxInnerRadius;
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

        // POWER SPAWN
        SpawnPower();

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // Checks if the enemy dies
        Die();


        // LIGHTS /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Light sprite is always the same as the boss pos
        summoningStance.transform.position = transform.position + new Vector3(0f, 0.30f, 0f);

        // Controls the screen light
        screenLight.pointLightOuterRadius = outerRadius;
        screenLight.pointLightInnerRadius = innerRadius;
        if(usingPowerAbility)
        {   // reduces player screen light
            if (outerRadius > minOuterRadius) outerRadius -= 12f * Time.deltaTime;
            if (innerRadius > minInnerRadius) innerRadius -= 6f * Time.deltaTime;

            // If the light isn't equal to player position, it will move slowly towards the player
            if (screenLight.transform.position != p1.transform.position + new Vector3(0f, 0.3f, 0f))
            {
                Vector3 moveLight = Vector3.MoveTowards(screenLight.transform.position,
                                                    p1.transform.position + new Vector3(0f, 0.3f, 0f), 500f * Time.deltaTime);
                screenLight.transform.position = moveLight;
            }
            else
                screenLight.transform.position = p1.transform.position + new Vector3(0f, 0.3f, 0f);
        }
        else
        {
            // augments player screen light
            if (outerRadius < maxOuterRadius) outerRadius += 12f * Time.deltaTime;
            if (innerRadius < maxInnerRadius) innerRadius += 6f * Time.deltaTime;

            // If the light isn't centered, it will move slowly towards the center
            if (screenLight.transform.position != Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)))
            {
                Vector3 moveLight = Vector3.MoveTowards(screenLight.transform.position, 
                                                    Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)), 500f * Time.deltaTime);
                screenLight.transform.position = moveLight;
            }
            else
                screenLight.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        }
    }

    private void SummoningStancePrepare()
    {   
        // gets invulnerable and turns the sprite into a light
        originalY           = transform.position.y;
        transform.position  = new Vector3(transform.position.x, originalY, transform.position.z);
        col.enabled         = false;
        sprite.enabled      = false;
        summoningStance.SetActive(true);
        usingPowerAbility   = true;
        // Enables positions to show a light
        position1.gameObject.SetActive(true);
        position2.gameObject.SetActive(true);
    }
    private void EndSummonStance()
    {   // goes back to normal
        col.enabled         = true;
        sprite.enabled      = true;
        summoningStance.SetActive(false);
        usingPowerAbility   = false;
        // Fisables positions to show a light
        position1.gameObject.SetActive(false);
        position2.gameObject.SetActive(false);
    }

    private void SpawnPower()
    {
        // Starts summon stance count down
        spawnCounter -= Time.deltaTime;
        if (spawnCounter < 0)
        {
            SummoningStancePrepare();

            // RANDOM POSITIONS //
            float randomPos1;
            float randomPos2;
            if (getRandNumbers)
            {
                // gets 2 random numbers, from min pos to max pos
                randomPos1 = Random.Range(minRange.position.x, maxRange.position.x);
                position1.position = new Vector3(randomPos1, position1.position.y, position1.rotation.z);
                randomPos2 = Random.Range(minRange.position.x, maxRange.position.x);
                position2.position = new Vector3(randomPos2, position2.position.y, position2.rotation.z);
                // Only gets the numbers once
                getRandNumbers = false;
            }

            // Starts power countdown
            usePowerCounter -= Time.deltaTime;
            if (usePowerCounter < 3)
            {
                // Uses power ability
                if (canUsePowerAbility)
                {
                    Instantiate(powerSpawnRight, position1.position, powerSpawnRight.transform.rotation);
                    Instantiate(powerSpawnLeft, position2.position, powerSpawnLeft.transform.rotation);
                    canUsePowerAbility = false;
                }
                // Returns to normal after the ability is over
                if (usePowerCounter < 0)
                {
                    EndSummonStance();
                    // Resets variables
                    canUsePowerAbility  = true;
                    getRandNumbers      = true;
                    usePowerCounter     = usePowerDelay;
                    spawnCounter        = spawnCounterDelay;
                }
            }
        }
    }

    protected override void AimCheck()
    {
        if (p1 != null)
        {
            // If boss isn't using the ability
            if (usingPowerAbility == false)
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
    }

    protected override void Die()
    {
        // ALIVE --------------------------------------------------------------------------------------
        if (!(Stats.IsAlive))
        {
            // Considers boss as defeated
            LevelManager.BossDefeated = true;

            Instantiate(deathSpawn, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);

            Destroy(gameObject);
        }
    }
}
