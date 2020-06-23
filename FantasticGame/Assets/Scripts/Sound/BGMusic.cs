using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class BGMusic : MonoBehaviour
{
    private static AudioSource audioSource;
    private float musicVolume = 0.1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sets volume = music volume
        audioSource.volume = musicVolume;
    }

    public void SetVolume(float volume)
    {
        // Controls sound volume
        musicVolume = volume;
    }
}
