using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // RANGED
    [SerializeField] Transform          magicPosition;
    [SerializeField] Transform          crouchedMagicPosition;
    [SerializeField] GameObject         magicPrefab;

    // MELEE
    [SerializeField] Transform          meleePosition;
    [SerializeField] GameObject         meleePrefab;
    [SerializeField] GameObject         meleeAttackTemporary;

    // SHIELD
    [SerializeField] Transform          shieldPosition;
    [SerializeField] Transform          crouchedShieldPosition;
    [SerializeField] GameObject         shieldPrefab;
    private bool                        canUseShield;

    // SWOOPING EVIL
    [SerializeField] Transform  swoopingPosition;
    [SerializeField] GameObject swoopingPrefab;
    [SerializeField] GameObject swoopingSpawnerPrefab;


    // LAYERS
    [SerializeField] LayerMask          treasureLayer;
    [SerializeField] LayerMask          enemyLayer, enemyAmmunitionLayer, meleeEnemyLayer;
    [SerializeField] LayerMask          onGroundLayers;

    // CAMERA
    public CameraShake                  CameraShake         { get; private set; }
    [SerializeField] float              shakeTime;
    [SerializeField] float              shakeForce;
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
    public Vector2                      ShieldPosition      { get; private set; }
    public Vector2                      MagicPosition       { get; private set; }
    public LevelManager                 Manager             { get; private set; }

    // CHEATS
    [SerializeField] bool           godMode;
    [SerializeField] bool           fly;
    [SerializeField] bool           infiniteMana;


    private void Awake()
    {
        Stats =     new Stats();
        animator =  GetComponent<Animator>();
        Movement =  GetComponent<PlayerMovement>();
        CameraShake = FindObjectOfType<CameraShake>();
        Manager =   FindObjectOfType<LevelManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
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
        canScreenShake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused == false)
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
            MagicPosition = magicPosition.position;
            // ---------------------------------------------------------------------------------------------



            // SHIELD --------------------------------------------------------------------------------------
            ShieldPosition = shieldPosition.position;
            UsingShield = false;

            canUseShield = CurrentMana > 5f ? true : false;

            if (Movement.OnGround && pressShield && canUseShield)
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
                if (Stats.CanUseSpell() && Stats.CanRangeAttack && !UsingShield)
                    Shoot();

            // ---------------------------------------------------------------------------------------------



            // MELEE ATTACK -------------------------------------------------------------------------------
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            if (Stats.CanMeleeAttack == false)
                Stats.MeleeAttackCounter -= Time.deltaTime;
            if (Stats.MeleeAttackCounter < 0)
            {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
                Stats.MeleeAttackCounter = Stats.MeleeAttackDelay;
                Stats.CanMeleeAttack = true;
            }

            if (Input.GetButtonDown("Fire1"))
                if (Stats.CanMeleeAttack)
                {
                    MeleeAttack();
                }
            // ---------------------------------------------------------------------------------------------



            // SWOOPING EVIL ------------------------------------------------------------------------------
            Collider2D swoopingCheck = Physics2D.OverlapCircle(swoopingPosition.position, 0.2f, onGroundLayers);
            if (Input.GetButtonDown("Fire3") && Movement.OnGround && SwoopingEvilPlatform.isAlive == false && swoopingCheck == null)
            {
                Instantiate(swoopingSpawnerPrefab, swoopingPosition.position, transform.rotation);
                Instantiate(swoopingPrefab, swoopingPosition.position, transform.rotation);

            }
            // Kills swooping evil if it's pressed again
            if (Input.GetButtonDown("Fire3") && Movement.OnGround && SwoopingEvilPlatform.isAlive) 
                SwoopingEvilPlatform.isAlive = false;
            // ---------------------------------------------------------------------------------------------


            // CHEATS
            if (godMode)
            {
                Movement.Invulnerable = true;
                Movement.invulnerableHP = 100f;
            }
            if (fly) if (Input.GetButton("Jump")) Movement.Rb.gravityScale = 0f;
            if (infiniteMana) Stats.CurrentMana = Stats.MaxMana;
        }
    }



    void Shoot()
    {
        RangedAttacked = true;
        animator.SetBool("rangedAttack", true);
        Stats.CanRangeAttack = false;
        Stats.SpendMana();
        // When Crouched
        if (Movement.CrouchGetter) Instantiate(magicPrefab, crouchedMagicPosition.position, magicPosition.rotation);
        else Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
    }

    void MeleeAttack()
    {
        animator.SetBool("attack", true);
        Stats.CanMeleeAttack = false;
        Instantiate(meleeAttackTemporary, meleePosition.position, transform.rotation);

        Collider2D[] treasureHit = Physics2D.OverlapCircleAll(meleePosition.position, Stats.MeleeAttackRange, treasureLayer);
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(meleePosition.position, Stats.MeleeAttackRange, enemyLayer);
        Collider2D[] meleeEnemyHit = Physics2D.OverlapCircleAll(meleePosition.position, Stats.MeleeAttackRange, meleeEnemyLayer);

        foreach (Collider2D treasure in treasureHit)
        {
            Instantiate(meleePrefab, treasure.GetComponent<Rigidbody2D>().position, transform.rotation);
            treasure.GetComponent<Treasure>().Stats.TakeDamage(Stats.MeleeDamage);
        }
        foreach (Collider2D enemy in enemyHit)
        {
            Instantiate(meleePrefab, enemy.GetComponent<Rigidbody2D>().position, transform.rotation);
            enemy.GetComponent<Enemy>().Stats.TakeDamage(Stats.MeleeDamage);
        }
        foreach (Collider2D enemy in meleeEnemyHit)
        {
            Instantiate(meleePrefab, enemy.GetComponent<Rigidbody2D>().position + new Vector2( 0f, 0.4f), transform.rotation);
            enemy.GetComponent<EnemyMelee>().Stats.TakeDamage(Stats.MeleeDamage);
        }
    }

    void Shield()
    {
        if (Physics2D.OverlapCircle(shieldPosition.position, 0.1f, enemyAmmunitionLayer)) StartCoroutine(CameraShake.Shake(0.015f, 0.04f));
        if (Movement.IsCrouched) Instantiate(shieldPrefab, crouchedShieldPosition.position, transform.rotation);
        else Instantiate(shieldPrefab, shieldPosition.position, transform.rotation);
        UsingShield = true;
        Stats.CurrentMana -= 10f * Time.deltaTime;
    }


}





