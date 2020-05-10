﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform      magicPosition;
    [SerializeField] GameObject     magicPrefab;


    [SerializeField] Transform      meleePosition;
    [SerializeField] GameObject     meleePrefab;

    [SerializeField] Transform shieldPosition;
    [SerializeField] GameObject     shieldPrefab;

    [SerializeField] LayerMask      treasureLayer;
    [SerializeField] LayerMask      enemyLayer;


    public Stats                    stats           { get; private set; }
    private Animator                animator;
    private PlayerMovement          movement;

    
    public float                    CurrentMana     { get; set; }
    public float                    CurrentHP       { get; set; }
    public bool                     usingShield     { get; private set; }
    public Vector2                  ShieldPosition  { get; private set; }
    private bool                    canUseShield;


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

        stats.MeleeDamage = 30f;
        stats.CanMeleeAttack = true;
        stats.MeleeAttackRange = 0.15f;
        stats.MeleeAttackDelay = 0.45f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused == false)
        {
            // UPDATE VARIABLES ----------------------------------------------------------------------------
            CurrentMana = stats.CurrentMana;
            CurrentHP = stats.CurrentHP;
            stats.RegenMana();
            animator.SetBool("attack", false);
            animator.SetBool("rangedAttack", false);
            // ---------------------------------------------------------------------------------------------

            // SHIELD --------------------------------------------------------------------------------------
            ShieldPosition = shieldPosition.position;
            usingShield = false;
            canUseShield = stats.CurrentMana > 1f ? true : false;

            if (movement.onGround && Input.GetButton("Fire3") && canUseShield)
            {             
                Shield();
            }


            // RANGED ATTACK -------------------------------------------------------------------------------
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            if (stats.CanRangeAttack == false)
                stats.RangedAttackCounter -= Time.deltaTime;
            if (stats.RangedAttackCounter < 0)
            {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
                stats.RangedAttackCounter = stats.RangedAttackDelay;
                stats.CanRangeAttack = true;
            }

            if (Input.GetButtonDown("Fire2"))
                if (stats.CanUseSpell() && stats.CanRangeAttack)
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
                stats.Die(gameObject);
            }
        }
    }



    void Shoot()
    {
        animator.SetBool("rangedAttack", true);
        stats.CanRangeAttack = false;
        stats.SpendMana();
        Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
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
        Instantiate(shieldPrefab, shieldPosition.position, transform.rotation);
        usingShield = true;
        stats.CurrentMana -= 10f * Time.deltaTime;
    }


}




