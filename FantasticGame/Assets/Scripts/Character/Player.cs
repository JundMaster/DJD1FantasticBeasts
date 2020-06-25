using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // RANGED
    [SerializeField] private Transform      magicPosition;
    [SerializeField] private GameObject     magicPrefab;

    // MELEE
    [SerializeField] private Transform      meleePosition;
    [SerializeField] private GameObject     meleeHitPrefab;
    [SerializeField] private Transform      meleeTrailPosition;
    private float   trailInitiaPos;
    private float   trailFinalPos;
    public float    trailCurrentPos    { get; private set; }
    public bool     Attacking   { get; private set; }
    private bool    maxReached;
    private bool    minReached;

    // SHIELD
    [SerializeField] private Transform      shieldPosition;
    [SerializeField] private GameObject     shieldPrefab;
    private bool                            canUseShield;

    // SWOOPING EVIL
    [SerializeField] private Transform      swoopingPosition;
    [SerializeField] private GameObject     swoopingPrefab;
    [SerializeField] private GameObject     swoopingSpawnerPrefab;

    // LOOKING
    public bool LookingUp       { get; private set; }
    public bool LookingDown     { get; private set; }

    // LAYERS
    [SerializeField] private LayerMask  treasureLayer;
    [SerializeField] private LayerMask  enemyLayer, enemyAmmunitionLayer;
    [SerializeField] private LayerMask  onGroundLayers;
    [SerializeField] private LayerMask  surpriseBoxLayer;

    // CAMERA
    public CameraShake                  CameraShake         { get; private set; }
    [SerializeField] private float      shakeTime;
    [SerializeField] private float      shakeForce;
    private bool                        canScreenShake;

    // ANIMATOR
    private Animator                    animator;

    // GET SETTERS
    public Stats                        Stats               { get; private set; }
    public PlayerMovement               Movement            { get; private set; }
    public float                        CurrentMana         { get; set; }
    public float                        CurrentHP           { get; set; }
    public bool                         RangedAttacked      { get; private set; }
    public bool                         UsingShield         { get; private set; }
    private float                       shieldSoundTimer;
    private float                       shieldSoundDelay;
    public Vector2                      ShieldPosition      { get; private set; }
    public LevelManager                 Manager             { get; private set; }

    // CHEATS
    [SerializeField] public bool    GodMode         { get; set; }
    [SerializeField] public bool    InfiniteMana    { get; set; }
    [SerializeField] private bool   fly;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        Movement = GetComponent<PlayerMovement>();
        CameraShake = FindObjectOfType<CameraShake>();
        Manager = FindObjectOfType<LevelManager>();

        Stats = new Stats();

        // STATS
        Stats.IsAlive       = true;
        Stats.MaxMana       = 100;
        Stats.CurrentMana   = Stats.MaxMana;
        Stats.AttackManaCost = 5f;
        Stats.ManaRegen     = 0.5f;
        Stats.MaxHP         = 100;
        Stats.CurrentHP     = Stats.MaxHP;

        // RANGED
        Stats.RangedDamage      = 50f;
        Stats.CanRangeAttack    = true;
        Stats.RangedAttackDelay = 0.5f;
        Stats.RangedAttackCounter = Stats.RangedAttackDelay;

        // MELEE
        Stats.MeleeDamage       = 30f;
        Stats.CanMeleeAttack    = true;
        Stats.MeleeAttackRange  = 0.15f;
        Stats.MeleeAttackDelay  = 0.45f;
        Stats.MeleeAttackCounter = Stats.MeleeAttackDelay;

        // ETC
        LookingDown = false;
        LookingUp   = false;
        canScreenShake = true;

        shieldSoundDelay = 0.2f;

        // TRAIL
        trailInitiaPos  = 0.2f;
        trailFinalPos   = 0.93f;
        meleeTrailPosition.localPosition = new Vector3(trailInitiaPos, 0.3f, 0f);
        trailCurrentPos = meleeTrailPosition.localPosition.x;
        Attacking = false;
        maxReached = false;
        minReached = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused == false && Respawn_GameOverMenu.InRespawnMenu == false && IntroScene.INTROSCENE == false)
        {
            // UPDATE VARIABLES ----------------------------------------------------------------------------
            CurrentMana = Stats.CurrentMana;
            CurrentHP = Stats.CurrentHP;
            if (Stats.CurrentHP < 0)
            {
                if (Movement.ropeSprite != null) Movement.ropeSprite.SetActive(false);
                Stats.IsAlive = false;
            }
            RangedAttacked = false;
            Stats.RegenMana();
            animator.SetBool("attack", false);
            animator.SetBool("rangedAttack", false);
            bool pressShield = Input.GetKey("s");
            if (Input.GetKey("up"))
                LookingUp = true;
            else 
                LookingUp = false;
            if (Input.GetKey("down") && Movement.OnGround)
                LookingDown = true;
            else
                LookingDown = false;
            // ---------------------------------------------------------------------------------------------

            // SHIELD --------------------------------------------------------------------------------------
            ShieldPosition = shieldPosition.position;
            UsingShield = false;

            canUseShield = CurrentMana > 5f ? true : false;

            if (Movement.OnGround && pressShield && canUseShield && !Movement.CrouchGetter)
            {
                Shield();
            }
            // ---------------------------------------------------------------------------------------------


            // RANGED ATTACK -------------------------------------------------------------------------------
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            if (Stats.CanRangeAttack == false)
            {
                Stats.RangedAttackCounter -= Time.deltaTime;
                if (Stats.RangedAttackCounter < 0.45f && canScreenShake) // SCREEN SHAKE WITH DELAY
                {
                    StartCoroutine(CameraShake.Shake(shakeTime, shakeForce));
                    canScreenShake = false;
                }
            }
            if (Stats.RangedAttackCounter < 0)
            {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
                Stats.RangedAttackCounter = Stats.RangedAttackDelay;
                Stats.CanRangeAttack = true;
                canScreenShake = true;
            }

            if (Input.GetButtonDown("Fire2"))
                if (Stats.CanUseSpell() && Stats.CanRangeAttack && !UsingShield && !Movement.CrouchGetter && !Movement.usingRope)
                    animator.SetBool("rangedAttack", true); // START ANIMATION
            // ---------------------------------------------------------------------------------------------


            // MELEE ATTACK -------------------------------------------------------------------------------
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            if (Stats.CanMeleeAttack == false)
                Stats.MeleeAttackCounter -= Time.deltaTime;
            if (Stats.MeleeAttackCounter < 0)
            {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
                Stats.MeleeAttackCounter = Stats.MeleeAttackDelay;
                Stats.CanMeleeAttack = true;

                Attacking = false; // FOR TRAIL
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (Stats.CanMeleeAttack && !UsingShield && !Movement.CrouchGetter && !Movement.usingRope)
                {
                    MeleeAttack();
                }
            }

            // TRAIL /////////////////////////////////
            // Keeps refreshing trail current position
            trailCurrentPos = meleeTrailPosition.localPosition.x;
            if (Attacking)
            {
                if (meleeTrailPosition.localPosition.x < trailFinalPos)
                {
                    if (minReached)
                        trailCurrentPos += 10f * Time.deltaTime;
                }
                if (meleeTrailPosition.localPosition.x > trailFinalPos)
                {
                    minReached = false;
                    maxReached = true;
                }
                if (meleeTrailPosition.localPosition.x > trailInitiaPos)
                {
                    if (maxReached)
                    {
                        if (meleeTrailPosition.localPosition.x > trailInitiaPos)
                            trailCurrentPos -= 10f * Time.deltaTime;
                    }
                }
            }
            else
            {
                minReached = true;
                maxReached = false;
            }
            meleeTrailPosition.localPosition = new Vector3(trailCurrentPos, 0.26f, 0f);
            // ---------------------------------------------------------------------------------------------


            // SWOOPING EVIL ------------------------------------------------------------------------------
            Collider2D swoopingCheck = Physics2D.OverlapCircle(swoopingPosition.position, 0.2f, onGroundLayers);
            if (Input.GetButtonDown("Fire3") && Movement.OnGround && SwoopingEvilPlatform.IsAlive == false && swoopingCheck == null)
            {
                Instantiate(swoopingSpawnerPrefab, swoopingPosition.position, transform.rotation);
                Instantiate(swoopingPrefab, swoopingPosition.position, transform.rotation);

            }
            // Kills swooping evil if it's pressed again
            if (Input.GetButtonDown("Fire3") && Movement.OnGround && SwoopingEvilPlatform.IsAlive) 
                SwoopingEvilPlatform.IsAlive = false;
            // ---------------------------------------------------------------------------------------------


            // CHEATS --------------------------------------------------------------------------------------
            if (GodMode) Stats.CurrentHP = 100f;
            if (fly) if (Input.GetButton("Jump")) Movement.Rb.gravityScale = 0f;
            if (InfiniteMana) Stats.CurrentMana = Stats.MaxMana;
            // ---------------------------------------------------------------------------------------------
        }
    }

    // CALLED ON ANIMATION // ANIMATION EVENT
    // Attacks, sets animation & sound, starts a timer on update, spends mana, instantiates the shoot prefab
    void Shoot()
    {   
        //Sound
        SoundManager.PlaySound(AudioClips.magicAttack);
        // Attack delay + spend mana
        RangedAttacked = true;
        Stats.CanRangeAttack = false;
        Stats.SpendMana();
        // Instantiates
        Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
    }

    // Attacks, sets animation, starts a timer on update, instantiates the attack prefab
    void MeleeAttack()
    {
        Attacking = true; // FOR TRAIL
        animator.SetBool("attack", true);
        Stats.CanMeleeAttack = false;
        SoundManager.PlaySound(AudioClips.melee);


        Collider2D[] treasureHit = Physics2D.OverlapCircleAll(meleePosition.position, 0.465f, treasureLayer);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(meleePosition.position, 0.465f, enemyLayer);
        Collider2D[] surpriseBoxes = Physics2D.OverlapCircleAll(meleePosition.position, 0.465f, surpriseBoxLayer);

        foreach (Collider2D treasure in treasureHit)
        {
            SoundManager.PlaySound(AudioClips.enemyHit);
            Instantiate(meleeHitPrefab, treasure.GetComponent<Rigidbody2D>().position, transform.rotation);
            treasure.GetComponent<Treasure>().Stats.TakeDamage(Stats.MeleeDamage);
        }
        foreach (Collider2D enemy in enemies)
        {
            SoundManager.PlaySound(AudioClips.enemyHit);
            Instantiate(meleeHitPrefab, enemy.GetComponent<Rigidbody2D>().position + new Vector2(0f, 0.4f), transform.rotation);
            enemy.GetComponent<EnemyBase>().Stats.TakeDamage(Stats.MeleeDamage);
        }
        foreach (Collider2D box in surpriseBoxes)
        {
            SoundManager.PlaySound(AudioClips.enemyHit);
            Instantiate(meleeHitPrefab, box.GetComponent<Rigidbody2D>().position + new Vector2(0f, 0.4f), transform.rotation);
            box.GetComponent<SurpriseBox>().Stats.TakeDamage(Stats.MeleeDamage);
        }
    }

    // Uses shield, spends mana, instantiates the shield prefab
    void Shield()
    {
        if (Physics2D.OverlapCircle(shieldPosition.position, 0.1f, enemyAmmunitionLayer))
        {
            StartCoroutine(CameraShake.Shake(0.015f, 0.04f));
        }
        Instantiate(shieldPrefab, shieldPosition.position, transform.rotation);
        UsingShield = true;
        Stats.CurrentMana -= 10f * Time.deltaTime;

        // Plays shield sound with a delay
        if (UsingShield)
        {
            shieldSoundTimer -= Time.deltaTime;
        }
        if (shieldSoundTimer < 0)
        {
            SoundManager.PlaySound(AudioClips.shield); // plays sound
            shieldSoundTimer = shieldSoundDelay;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleePosition.position, 0.465f);
    }

}





