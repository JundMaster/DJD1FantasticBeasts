using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMelee : MonoBehaviour
{
    [SerializeField] Transform meleeWeapon;
    [SerializeField] int damage = 25;
    [SerializeField] float attackRange = 0.1f;
    [SerializeField] LayerMask hittableLayers;
    [SerializeField] float maxTimeDelay = 0.45f;
    float timeDelay;
    bool canAttack;

    [SerializeField] GameObject hitProjectile;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        timeDelay = maxTimeDelay;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("attack", false);


        if (canAttack == false)
        {
            // Everytime the player attacks, it starts a timer and sets canAttack to false
            timeDelay -= Time.deltaTime;
        }
        // If timeDelay gets < 0, the character can attack again
        if (timeDelay < 0)
        {   // Sets time delay to maxTimeDelayAgain
            timeDelay = maxTimeDelay;
            canAttack = true;
        }


        if (PauseMenu.gamePaused == false)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (canAttack)
                {
                    Attack();
                }
            }
        }
    }

    void Attack()
    {
        canAttack = false;

        anim.SetBool("attack", true);

        Collider2D[] hit = Physics2D.OverlapCircleAll(meleeWeapon.position, attackRange, hittableLayers);

        foreach (Collider2D treasure in hit)
        {
            Instantiate(hitProjectile, treasure.GetComponent<Rigidbody2D>().position, transform.rotation);
            treasure.GetComponent<Treasure>().takeDamage(damage);     
        }
    }

    /*
    private void OnDrawGizmos()
    {
        if (meleeWeapon == null)
            return;
        Gizmos.color = new Color(1, 0.92f, 0.015f, 0.1f);
        Gizmos.DrawSphere(meleeWeapon.position, attackRange);
    }
    */
}
