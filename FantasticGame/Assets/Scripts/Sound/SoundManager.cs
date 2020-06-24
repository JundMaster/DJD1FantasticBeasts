using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SoundManager : MonoBehaviour
{
    // Sounds
    private static AudioClip jump;
    private static AudioClip jumpLanding;
    private static AudioClip enemyHit;
    private static AudioClip magicAttack;
    private static AudioClip powerUp;
    private static AudioClip ropeHit;
    private static AudioClip swoopingPlatform;
    private static AudioClip shield;
    private static AudioClip niffler;
    private static AudioClip melee;
    private static AudioClip walk;
    private static AudioClip textPopUp;
    private static AudioClip buttonSelected;
    private static AudioClip buttonScroll;



    private static AudioSource audioSource;
    private float effectsVolume = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        jump            = Resources.Load<AudioClip>("Sounds/jump");
        jumpLanding     = Resources.Load<AudioClip>("Sounds/jumpLanding");
        enemyHit        = Resources.Load<AudioClip>("Sounds/enemyHit");
        magicAttack     = Resources.Load<AudioClip>("Sounds/magicAttack");
        powerUp         = Resources.Load<AudioClip>("Sounds/powerup");
        ropeHit         = Resources.Load<AudioClip>("Sounds/ropeHit");
        swoopingPlatform = Resources.Load<AudioClip>("Sounds/swoopingPlatform");
        shield          = Resources.Load<AudioClip>("Sounds/shield");
        niffler         = Resources.Load<AudioClip>("Sounds/niffler");
        melee           = Resources.Load<AudioClip>("Sounds/melee");
        walk            = Resources.Load<AudioClip>("Sounds/walk");
        textPopUp = Resources.Load<AudioClip>("Sounds/textPopUp");
        buttonSelected = Resources.Load<AudioClip>("Sounds/buttonSelected");
        buttonScroll = Resources.Load<AudioClip>("Sounds/buttonScroll");
    }

    private void Update()
    {
        audioSource.volume = effectsVolume;
    }

    // Method used to play a sound
    public static void PlaySound(AudioClips clip)
    {
        switch (clip)
        {
            case AudioClips.jump:
                audioSource.PlayOneShot(jump, 1f);
                break;
            case AudioClips.jumpLanding:
                audioSource.PlayOneShot(jumpLanding, 1f);
                break;
            case AudioClips.hit:
                audioSource.PlayOneShot(enemyHit, 1f);
                break;
            case AudioClips.enemyHit:
                audioSource.PlayOneShot(enemyHit, 0.6f);
                break;
            case AudioClips.magicAttack:
                audioSource.PlayOneShot(magicAttack, 0.4f);
                break;
            case AudioClips.powerUp:
                audioSource.PlayOneShot(powerUp, 2f);
                break;
            case AudioClips.ropeHit:
                audioSource.PlayOneShot(ropeHit, 0.6f);
                break;
            case AudioClips.swoopingPlatform:
                audioSource.PlayOneShot(swoopingPlatform, 1.2f);
                break;
            case AudioClips.shield:
                audioSource.PlayOneShot(shield, 0.4f);
                break;
            case AudioClips.niffler:
                audioSource.PlayOneShot(niffler, 0.8f);
                break;
            case AudioClips.melee:
                audioSource.PlayOneShot(melee, 0.8f);
                break;
            case AudioClips.walk:
                audioSource.PlayOneShot(walk, 0.6f);
                break;
            case AudioClips.textPopUp:
                audioSource.PlayOneShot(textPopUp, 2f);
                break;
            case AudioClips.buttonSelected:
                audioSource.PlayOneShot(buttonSelected, 0.4f);
                break;
            case AudioClips.buttonScroll:
                audioSource.PlayOneShot(buttonScroll, 1.2f);
                break;
        }
    }


    public void SetVolume(float volume)
    {
        // Controls sound volume
        effectsVolume = volume;
    }
}
