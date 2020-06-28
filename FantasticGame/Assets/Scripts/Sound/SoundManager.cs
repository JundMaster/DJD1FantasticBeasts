using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SoundManager : MonoBehaviour
{
    // Sounds
    private static AudioClip[] sounds;

    private static AudioSource audioSource;
    private float effectsVolume = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        sounds = new AudioClip[14];

        sounds[0]   = Resources.Load<AudioClip>("Sounds/jump");
        sounds[1]   = Resources.Load<AudioClip>("Sounds/jumpLanding");
        sounds[2]   = Resources.Load<AudioClip>("Sounds/enemyHit");
        sounds[3]   = Resources.Load<AudioClip>("Sounds/magicAttack");
        sounds[4]   = Resources.Load<AudioClip>("Sounds/powerup");
        sounds[5]   = Resources.Load<AudioClip>("Sounds/ropeHit");
        sounds[6]   = Resources.Load<AudioClip>("Sounds/swoopingPlatform");
        sounds[7]   = Resources.Load<AudioClip>("Sounds/shield");
        sounds[8]   = Resources.Load<AudioClip>("Sounds/niffler");
        sounds[9]   = Resources.Load<AudioClip>("Sounds/melee");
        sounds[10]  = Resources.Load<AudioClip>("Sounds/walk");
        sounds[11]  = Resources.Load<AudioClip>("Sounds/textPopUp");
        sounds[12]  = Resources.Load<AudioClip>("Sounds/buttonSelected");
        sounds[13]  = Resources.Load<AudioClip>("Sounds/buttonScroll");
    }

    private void Update()
    {
        // Sets volume = to effects slider volume on pause menu
        audioSource.volume = effectsVolume;
    }

    public void SetVolume(float volume)
    {
        // Controls sound volume
        effectsVolume = volume;
    }

    // Plays a sound depending on the clip parameter
    public static void PlaySound(AudioClips clip)
    {
        switch (clip)
        {
            case AudioClips.jump:
                audioSource.PlayOneShot(sounds[0], 1f);
                break;
            case AudioClips.jumpLanding:
                audioSource.PlayOneShot(sounds[1], 1f);
                break;
            case AudioClips.hit:
                audioSource.PlayOneShot(sounds[2], 1f);
                break;
            case AudioClips.enemyHit:
                audioSource.PlayOneShot(sounds[2], 0.6f);
                break;
            case AudioClips.magicAttack:
                audioSource.PlayOneShot(sounds[3], 0.4f);
                break;
            case AudioClips.powerUp:
                audioSource.PlayOneShot(sounds[4], 2f);
                break;
            case AudioClips.ropeHit:
                audioSource.PlayOneShot(sounds[5], 0.6f);
                break;
            case AudioClips.swoopingPlatform:
                audioSource.PlayOneShot(sounds[6], 1.2f);
                break;
            case AudioClips.shield:
                audioSource.PlayOneShot(sounds[7], 0.4f);
                break;
            case AudioClips.niffler:
                audioSource.PlayOneShot(sounds[8], 0.8f);
                break;
            case AudioClips.melee:
                audioSource.PlayOneShot(sounds[9], 0.8f);
                break;
            case AudioClips.walk:
                audioSource.PlayOneShot(sounds[10], 0.6f);
                break;
            case AudioClips.textPopUp:
                audioSource.PlayOneShot(sounds[11], 2f);
                break;
            case AudioClips.buttonSelected:
                audioSource.PlayOneShot(sounds[12], 0.4f);
                break;
            case AudioClips.buttonScroll:
                audioSource.PlayOneShot(sounds[13], 1.2f);
                break;
        }
    }
}
