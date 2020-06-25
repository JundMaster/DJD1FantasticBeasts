using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMelee : EnemyBase
{
    // Editor stuff
    [SerializeField] private GameObject ammunitionHit;

    // Attack
    private Collider2D  atackingCollider;

    private bool attackAnimation;

    private void Start()
    {
        animator    = GetComponent<Animator>();
        p1          = FindObjectOfType<PlayerMovement>();

        Stats = new Stats
        {
            IsAlive     = true,
            CurrentHP   = HP,
            MaxHP       = HP,

            MeleeDamage     = enemyDamage,
            CanMeleeAttack  = false,
            MeleeAttackDelay = attackDelay
        };

        // initial position and movement timers
        startingPos     = transform.position;
        originalSpeed   = speed;

        waitingTimeCounter          = Random.Range(1f, 3f);

        // attack
        attackDelay         = 0f;
        //nextAttacksDelay    = 0.99f;
        attacking           = false;
    }

    private void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<PlayerMovement>();
        }

        if (healthBarRect)
        {
            healthBarRect.localScale = new Vector2(Stats.CurrentHP / Stats.MaxHP, 1f);
        }

        //  BACKSTAB ----------------------------------------------------------------------------------
        BackStabCheck();

        //  AIMING CHECK ------------------------------------------------------------------------------
        AimCheck();

        // Checks if the enemy dies
        base.Die();

        // ANIMATIONS --------------------------------------------------------------------------------
        Animations();
    }

    // Checks if the the player is in range and if there's an object between the enemy and player
    void AimCheck()
    {
        atackingCollider = Physics2D.OverlapBox(attackPosition.position, new Vector3(0.4f, 0.7f, 0f) , 0f, playerLayer);

        // If the collider has a player in range
        if (atackingCollider != null)
        {
            //Starts the atacking animation
            attacking = false;

            // Sets a timer to move, sets a timer to attack
            canMoveTimer = 0.70f;
            Stats.MeleeAttackDelay -= Time.deltaTime;

            // Timer reaches // Enemy Attacks
            if (Stats.MeleeAttackDelay < 0)
            {
                attacking = true; // FOR ANIMATOR // CALLS MELEE() // ANIMATION EVENT
                Stats.MeleeAttackDelay = attackDelay;
            }
        }
        else
        {
            attacking = false; // FOR ANIMATOR
            // If the player leaves max range, if the timer is less than 0, the enemy moves
            canMoveTimer -= Time.deltaTime;

            if (canMoveTimer <= 0)
            {
                Movement();
            }
        }
    }

    // Melee Attack // CALLED ON ANIMATOR // ANIMATION EVENT
    private void Melee()
    {
        SoundManager.PlaySound(AudioClips.magicAttack); // Plays sound

        // Only damages the player if the player is in range
        if (atackingCollider != null)
        {
            p1.player.Stats.TakeDamage(Stats.MeleeDamage);

            // Instantiates player hit depending on its position
            if (p1.CrouchGetter)
            {
                Instantiate(ammunitionHit, p1.transform.position + new Vector3(0f, 0.3f, 0f), p1.transform.rotation);
            }
            else
            {
                Instantiate(ammunitionHit, p1.transform.position + new Vector3(0f, 0.5f, 0f), p1.transform.rotation);
            }

            // Pushes the player back
            if (p1.transform.position.x > transform.position.x)
            {
                p1.Rb.AddForce(new Vector2(attackPushForce, 0f));
            }
            else if (p1.transform.position.x < transform.position.x)
            {
                p1.Rb.AddForce(new Vector2(-attackPushForce, 0f));
            }

            // Shakes the screen
            StartCoroutine(p1.player.CameraShake.Shake(0.025f, 0.08f));
        }
    }

    // prints enemy attack range on editor
    private void OnDrawGizmos()
    {
        if (transform.right.x > 0) Gizmos.DrawWireCube(attackPosition.position, new Vector3( 0.4f, 0.7f, 0f));
        if (transform.right.x < 0) Gizmos.DrawWireCube(attackPosition.position, new Vector3( -0.4f, 0.7f, 0f));
        Gizmos.DrawWireSphere(backStab.position, 0.15f);
    }
}





