using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFire : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] GameObject ammunitionSprite;
    [SerializeField] float maxTimeDelay = 0.5f;
    float timeDelay;
    bool canAttack;
    public static bool fire = false;


    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetBool("attack", false);  

        // Says if the character used a magic
        fire = false;

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
            if (CharacterInfo.hasMana)
            {
                if (canAttack)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        //anim.SetBool("attack", true);
                        Shoot();
                    }
                }
            }
        }
    }

    public void Shoot()
    {
        fire = true;
        canAttack = false;  
        Instantiate(ammunitionSprite, weapon.position, weapon.rotation);
    }
}
