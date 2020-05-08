using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform      magicPosition;
    [SerializeField] GameObject     magicSprite;


    [SerializeField] Transform      meleeWeapon;
    [SerializeField] GameObject     meleeHitSprite;


    [SerializeField] LayerMask      treasureLayer;
    [SerializeField] LayerMask      enemyLayer;


    private Stats                   stats;
    private Animator                animator;


    private void Awake()
    {
        stats = new Stats();
        animator = GetComponent<Animator>();
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
        stats.RangedAttacking = false;
        stats.RangedAttackDelay = 0.5f;

        stats.MeleeDamage = 25f;
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
            stats.RegenMana();
            stats.RangedAttacking = false;
            animator.SetBool("attack", false);

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
                stats.Die();
        }
    }



    void Shoot()
    {
        //animation.SetBool("rangedAttack", true);
        stats.RangedAttacking = true;
        stats.CanRangeAttack = false;
        stats.SpendMana();
        Instantiate(magicSprite, magicPosition.position, magicPosition.rotation);
    }

    void MeleeAttack()
    {
        animator.SetBool("attack", true);
        stats.CanMeleeAttack = false;
        
        Collider2D[] treasureHit = Physics2D.OverlapCircleAll(meleeWeapon.position, stats.MeleeAttackRange, treasureLayer);
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(meleeWeapon.position, stats.MeleeAttackRange, enemyLayer);

        foreach (Collider2D treasure in treasureHit)
        {
            Instantiate(meleeHitSprite, treasure.GetComponent<Rigidbody2D>().position, transform.rotation);
            treasure.GetComponent<Treasure>().takeDamage((int)stats.MeleeDamage);
        }
        foreach (Collider2D enemy in enemyHit)
        {
            Instantiate(meleeHitSprite, enemy.GetComponent<Rigidbody2D>().position, transform.rotation);
            enemy.GetComponent<Enemy>().takeDamage((int)stats.MeleeDamage);
        }
    }
}







/*
void OnCollisionEnter2D(Collision2D hitInfo)
{
    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

    if (enemy != null)
    {
        TakeDamage(enemy.damage);
    }
}
*/