using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class AmbientSoundManager : MonoBehaviour
{
    private static AudioSource audioSource;
    private float ambientVolume = 0.2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sets volume = music volume
        audioSource.volume = ambientVolume;
    }

    public void SetVolume(float volume)
    {
        // Controls sound volume
        ambientVolume = volume;
    }
}
