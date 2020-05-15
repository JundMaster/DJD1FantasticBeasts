using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform          magicPosition;
    [SerializeField] Transform          crouchedMagicPosition;
    [SerializeField] GameObject         magicPrefab;

    [SerializeField] Transform          meleePosition;
    [SerializeField] GameObject         meleePrefab;

    [SerializeField] Transform          shieldPosition;
    [SerializeField] Transform          crouchedShieldPosition;
    [SerializeField] GameObject         shieldPrefab;

    [SerializeField] LayerMask          treasureLayer;
    [SerializeField] LayerMask          enemyLayer, enemyAmmunitionLayer;

    [SerializeField] CameraShake        cameraShake;
    [SerializeField] float              shakeTime;
    [SerializeField] float              shakeForce;

    public Stats                        stats           { get; private set; }
    private Animator                    animator;
    public PlayerMovement               movement        { get; private set; }

    
    public float                    CurrentMana             { get; set; }
    public float                    CurrentHP               { get; set; }
    public bool                     RangedAttacked          { get; private set; }
    public bool                     usingShield             { get; private set; }
    public Vector2                  ShieldPosition          { get; private set; }
    public Vector2                  MagicPosition           { get; private set; }


    private bool                    canUseShield;
    private bool                    canScreenShake;

    [SerializeField] bool           godMode;
    [SerializeField] bool           fly;


    private void Awake()
    {
        stats = new Stats();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }


    // Start is called before the first frame update
    void Start()
    {
        stats.MaxMana = 100;
        stats.CurrentMana = stats.MaxMana;
        stats.AttackManaCost = 5f;
        stats.ManaRegen = 0.5f;
        stats.MaxHP = 100;
        stats.CurrentHP = stats.MaxHP;

        stats.RangedDamage = 50f;
        stats.CanRangeAttack = true;
        stats.RangedAttackDelay = 0.5f;
        stats.RangedAttackCounter = stats.RangedAttackDelay;

        stats.MeleeDamage = 30f;
        stats.CanMeleeAttack = true;
        stats.MeleeAttackRange = 0.15f;
        stats.MeleeAttackDelay = 0.45f;
        stats.MeleeAttackCounter = stats.MeleeAttackDelay;

        canScreenShake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused == false)
        {
            // UPDATE VARIABLES ----------------------------------------------------------------------------
            CurrentMana = stats.CurrentMana;
            CurrentHP = stats.CurrentHP;
            RangedAttacked = false;
            stats.RegenMana();
            animator.SetBool("attack", false);
            animator.SetBool("rangedAttack", false);
            bool pressShield = Input.GetKey("w") || Input.GetKey("up");
            MagicPosition = magicPosition.position;
            // ---------------------------------------------------------------------------------------------



            // SHIELD --------------------------------------------------------------------------------------
            ShieldPosition = shieldPosition.position;
            usingShield = false;

            canUseShield = CurrentMana > 5f ? true : false;

            if (movement.onGround && pressShield && canUseShield)
            {             
                Shield();
            }

            // RANGED ATTACK -------------------------------------------------------------------------------
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            if (stats.CanRangeAttack == false)
            {
                stats.RangedAttackCounter -= Time.deltaTime;
                if (stats.RangedAttackCounter < 0.45f && canScreenShake) // SCREEN SHAKE WITH DELAY
                {
                    StartCoroutine(cameraShake.Shake(shakeTime, shakeForce));
                    canScreenShake = false;
                }
            }
            if (stats.RangedAttackCounter < 0)
            {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
                stats.RangedAttackCounter = stats.RangedAttackDelay;
                stats.CanRangeAttack = true;
                canScreenShake = true;
            }

            if (Input.GetButtonDown("Fire2"))
                if (stats.CanUseSpell() && stats.CanRangeAttack && !usingShield)
                    Shoot();




            // MELEE ATTACK -------------------------------------------------------------------------------
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            if (stats.CanMeleeAttack == false)
                stats.MeleeAttackCounter -= Time.deltaTime;
            if (stats.MeleeAttackCounter < 0)
            {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
                stats.MeleeAttackCounter = stats.MeleeAttackDelay;
                stats.CanMeleeAttack = true;
            }

            if (Input.GetButtonDown("Fire1"))
                if (stats.CanMeleeAttack)
                {
                    MeleeAttack();
                }




            // ALIVE CONDITION ----------------------------------------------------------------------------
            if (!(stats.IsAlive))
            {
                Destroy(gameObject);
                stats.Die(gameObject);
            }

            if (godMode) stats.CurrentHP = 10000000000;
            if (fly) if (Input.GetButton("Jump")) movement.rb.gravityScale = 0f;
        }
    }



    void Shoot()
    {
        RangedAttacked = true;
        animator.SetBool("rangedAttack", true);
        stats.CanRangeAttack = false;
        stats.SpendMana();
        // When Crouched
        if (movement.crouchGetter) Instantiate(magicPrefab, crouchedMagicPosition.position, magicPosition.rotation);
        else Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
    }

    void MeleeAttack()
    {
        animator.SetBool("attack", true);
        stats.CanMeleeAttack = false;

        Collider2D[] treasureHit = Physics2D.OverlapCircleAll(meleePosition.position, stats.MeleeAttackRange, treasureLayer);
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(meleePosition.position, stats.MeleeAttackRange, enemyLayer);

        foreach (Collider2D treasure in treasureHit)
        {
            Instantiate(meleePrefab, treasure.GetComponent<Rigidbody2D>().position, transform.rotation);
            treasure.GetComponent<Treasure>().stats.TakeDamage(stats.MeleeDamage);
        }
        foreach (Collider2D enemy in enemyHit)
        {
            Instantiate(meleePrefab, enemy.GetComponent<Rigidbody2D>().position, transform.rotation);
            enemy.GetComponent<Enemy>().stats.TakeDamage(stats.MeleeDamage);
        }
    }

    void Shield()
    {
        if (Physics2D.OverlapCircle(shieldPosition.position, 0.1f, enemyAmmunitionLayer)) StartCoroutine(cameraShake.Shake(0.015f, 0.04f));
        if (movement.IsCrouched) Instantiate(shieldPrefab, crouchedShieldPosition.position, transform.rotation);
        else Instantiate(shieldPrefab, shieldPosition.position, transform.rotation);
        usingShield = true;
        stats.CurrentMana -= 10f * Time.deltaTime;

    }


}





