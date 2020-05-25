using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public Stats Stats { get; private set; }

    [SerializeField] Transform          meleePosition;


    [SerializeField] LayerMask          playerLayer;
    [SerializeField] LayerMask          groundLayer;
    [SerializeField] LayerMask          boxesAndwalls;

    [SerializeField] Transform          groundRangeCheck, groundCheck, wallCheck, backStab;


    [SerializeField] GameObject         ammunitionHit;
    // Drops
    [SerializeField] GameObject         healthPickUp, manaPickUp;

    [SerializeField] float  speed;       // WALKING SPEED
    [SerializeField] float  limitRange;  // RANGE FOR WALKING
    [SerializeField] float  backStabSize; // BACK STAB COLLIDER SIZE
    [SerializeField] float  HP;          // CURRENT HP

    [SerializeField] float  enemyDamage;     // ENEMY DAMAGE
    [SerializeField] float  attackPushForce;  // HOW MUCH WILL ENEMY PUSH THE PLAYER
    [SerializeField] int    lootChance;    // LOOT CHANCE 1 - 10

    //[SerializeField] bool   shooter; // ONLY FOR DEMO

    public float    Damage { get; private set; }
    public float    PushForce { get; private set; }

    bool            meleeAttack;
    float           attackDelay;

    float           originalSpeed;
    Vector2         startingPos;
    bool            limitWalkingRangeReached;
    Vector2         tempPosition;
    float           waitingTimeCounter;
    bool            backStabCheckerEnabled;

    float           canMoveTimer;

    Player p1;
    Animator animator;
    private void Awake()
    {
        Stats = new Stats();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Stats.IsAlive       = true;
        startingPos         = transform.position;
        Stats.CurrentHP     = HP;

        attackDelay = 0.75f;
        Stats.CanMeleeAttack    = false;
        Stats.MeleeAttackDelay  = attackDelay;
        meleeAttack             = false;


        Stats.MeleeDamage   = enemyDamage;
        Damage              = enemyDamage;
        PushForce           = attackPushForce;


        limitWalkingRangeReached    = false;
        //waitingTimeCounter        = waitingTime;
        waitingTimeCounter          = Random.Range(1f, 3f);


        backStabCheckerEnabled  = false;
        originalSpeed           = speed;
    }

    private void Update()
    {
        p1 = FindObjectOfType<Player>();


        // OTHER ATTACKS ANIMATION DELAY
        if (meleeAttack == true )
        {
            Stats.MeleeAttackDelay -= Time.deltaTime;
        }
        if (Stats.MeleeAttackDelay < 0)
        {   // If timeDelay gets < 0, sets timer back to initial delay again and the character can attack
            Stats.CanMeleeAttack = true;
            Stats.MeleeAttackDelay = attackDelay;
        }


        // Movement -----------------------------------------------------------------------------------
        // Attack delays for ranged and melee attacks
        if (Stats.CanMeleeAttack)     // Attack animation delay
        {
            Melee();
        }
        if (meleeAttack == false)
        {
            Movement();
        }



        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();



        // CHECKS IF PLAYER IS ON THE ENEMY'S BACK ----------------------------------------------------
        BackStabCheck();


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

        // ANIMATIONS --------------------------------------------------------------------------------
        animator.SetBool("attack", meleeAttack);
        animator.SetFloat("speed", speed);
    }

    
    // Checks if the player is behind the enemy, if this is true, rotates the enemy
    void BackStabCheck()
    {
        if (backStabCheckerEnabled)
        {
            Collider2D checkSurround = Physics2D.OverlapCircle(backStab.position, 0.5f, playerLayer);

            if (checkSurround && meleeAttack == false)
            {
                transform.Rotate(0f, 180f, 0f);
                if (limitWalkingRangeReached == true) limitWalkingRangeReached = false;
            }
        }
    }
    

    // Checks if the the player is in range and if there's an object between the enemy and player
    void AimCheck()
    {
        Collider2D aimTop;
        if (transform.rotation.y > 0) aimTop = Physics2D.OverlapBox(meleePosition.position, new Vector3(0.2f, 0.7f, 0.2f) , 0f, playerLayer);
        else aimTop = Physics2D.OverlapBox(meleePosition.position, new Vector3(0.15f, 0.7f, 0.2f), 0f, playerLayer);

        if (aimTop != null)
        {
            meleeAttack = true;
            speed = 0;
            // Sets canMoveTimer to attackDelay, to start counting in the beggining of the attack
            canMoveTimer = attackDelay;
        }
        else
        {
            // Only cancells attack and starts moving AFTER atacking
            canMoveTimer -= Time.deltaTime;
            if (canMoveTimer < 0)
            {
                if (limitWalkingRangeReached == false)
                {
                    meleeAttack = false;
                    speed = originalSpeed;
                }
            }
            
            // If player moves out of range, resets animation counter to initial value
            Stats.MeleeAttackDelay = attackDelay;
        }
    }

    // Melee Attack
    void Melee()
    {
        backStabCheckerEnabled = true;
        Stats.CanMeleeAttack = false;
        p1.Stats.TakeDamage(Damage);
        if (p1.Movement.CrouchGetter) Instantiate(ammunitionHit, p1.transform.position + new Vector3(0f, 0.3f, 0f), p1.transform.rotation);
        else Instantiate(ammunitionHit, p1.transform.position + new Vector3(0f, 0.5f, 0f), p1.transform.rotation);

        // Pushes the player
        if (p1.transform.position.x > transform.position.x)
            p1.Movement.Rb.AddForce(new Vector2(PushForce, 0f));
        else if (p1.transform.position.x < transform.position.x)
            p1.Movement.Rb.AddForce(new Vector2(-PushForce, 0f));
        StartCoroutine(p1.CameraShake.Shake(0.025f, 0.08f));
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
                limitWalkingRangeReached = false;
                speed = originalSpeed;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(meleePosition.position, new Vector3 ( 0.2f, 0.7f, 0.2f));
        Gizmos.DrawWireSphere(backStab.position, backStabSize);
    }
}





