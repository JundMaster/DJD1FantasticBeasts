using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMelee : MonoBehaviour
{
    [SerializeField] Transform meleeWeapon;
    [SerializeField] int damage;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask hittableLayers;


    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // Fixes a bug where character atacked on pause menu
        PauseMenu.gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("attack", false);

        if (PauseMenu.gamePaused == false)
        {
            if (Input.GetButtonDown("Fire1"))
            {  
                Attack();
            }
        }
        
    }

    void Attack()
    {
        anim.SetBool("attack", true);

        Collider2D[] hit = Physics2D.OverlapCircleAll(meleeWeapon.position, attackRange, hittableLayers);

        foreach (Collider2D treasure in hit)
        {
            treasure.GetComponent<Treasure>().takeDamage(damage);
            
        }
    }

    private void OnDrawGizmos()
    {
        if (meleeWeapon == null)
            return;
        Gizmos.color = new Color(1, 0.92f, 0.015f, 0.1f);
        Gizmos.DrawSphere(meleeWeapon.position, attackRange);
    }
}
