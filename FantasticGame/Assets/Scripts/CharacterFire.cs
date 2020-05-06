using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFire : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] GameObject ammunitionSprite;
    
    


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
            if (Input.GetButtonDown("Fire2"))
            {
                anim.SetBool("attack", true);
                Shoot();
            }
        }
    }

    void Shoot()
    {
         Instantiate(ammunitionSprite, weapon.position, weapon.rotation);
    }
}
