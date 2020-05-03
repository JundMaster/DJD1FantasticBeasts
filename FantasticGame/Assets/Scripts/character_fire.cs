using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_fire : MonoBehaviour
{
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
                anim.SetBool("attack", true);
            }
        }
    }
}
