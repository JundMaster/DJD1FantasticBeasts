using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SoundManager : MonoBehaviour
{
    // Sounds
    private static AudioClip jump;
    private static AudioClip jumpLanding;
    
    private static AudioClip hit;
    private static AudioClip enemyHit;
    private static AudioClip magicAttack;

    private static AudioClip powerUp;

    private static AudioClip ropeHit;
    private static AudioClip ropeGoing;
    private static AudioClip swoopingPlatform;

    private static AudioClip shield;

    private static AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        jump            = Resources.Load<AudioClip>("Sounds/jump");
        jumpLanding     = Resources.Load<AudioClip>("Sounds/jumpLanding");
        
        hit             = Resources.Load<AudioClip>("Sounds/hit");
        enemyHit        = Resources.Load<AudioClip>("Sounds/enemyHit");
        magicAttack     = Resources.Load<AudioClip>("Sounds/magicAttack");

        powerUp         = Resources.Load<AudioClip>("Sounds/powerup");
        
        ropeHit         = Resources.Load<AudioClip>("Sounds/ropeHit");
        ropeGoing       = Resources.Load<AudioClip>("Sounds/ropeGoing");
        swoopingPlatform = Resources.Load<AudioClip>("Sounds/swoopingPlatform");

        shield          = Resources.Load<AudioClip>("Sounds/shield");
    }

    // Method used to play a sound
    public static void PlaySound(AudioClips clip)
    {
        switch (clip)
        {
            case AudioClips.jump:
                audioSource.PlayOneShot(jump);
                break;
            case AudioClips.jumpLanding:
                audioSource.PlayOneShot(jumpLanding, 0.5f);
                break;
            case AudioClips.hit:
                audioSource.PlayOneShot(hit, 0.5f);
                break;
            case AudioClips.enemyHit:
                audioSource.PlayOneShot(enemyHit, 0.3f);
                break;
            case AudioClips.magicAttack:
                audioSource.PlayOneShot(magicAttack, 0.5f);
                break;
            case AudioClips.powerUp:
                audioSource.PlayOneShot(powerUp, 1);
                break;
            case AudioClips.ropeHit:
                audioSource.PlayOneShot(ropeHit, 0.7f);
                break;
            case AudioClips.ropeGoing:
                audioSource.PlayOneShot(ropeGoing);
                break;
            case AudioClips.swoopingPlatform:
                audioSource.PlayOneShot(swoopingPlatform, 0.6f);
                break;
            case AudioClips.shield:
                audioSource.PlayOneShot(shield, 1f);
                break;
        }
    }
}