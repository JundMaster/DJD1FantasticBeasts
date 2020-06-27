using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

sealed public class IntroScene_Level02 : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject textOnBlackScreen;

    public static bool CUTSCENE { get; set; } = false;

    // Player
    private Player p1;

    // Camera
    CameraFollow cam;

    private void Start()
    {
        CUTSCENE = true;

        p1 = FindObjectOfType<Player>();
        cam = FindObjectOfType<CameraFollow>();


        if (music) music.Stop();


        // Stops time and calls INTRO
        // Enables a black screen
        blackScreen.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(Intro());
      
    }

    void FixedUpdate()
    {
        // When the player passes through a certain point
        if (cam.cameraMoving)
        {
            Destroy(textOnBlackScreen);
        }

        // Destroys this object after the player moves
        if (p1.transform.position.x > 15f) Destroy(gameObject);
    }


    IEnumerator Intro()
    {
        while (true)
        {   // Controls the cutscene
            yield return new WaitForSecondsRealtime(3.7f);
            Time.timeScale = 1f;
            yield return new WaitForSeconds(0.5f);
            if (music) music.Play();
            yield return new WaitForSeconds(1f);
            break;
        }
        CUTSCENE = false;
    }
}
