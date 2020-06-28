using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

sealed public class IntroScene : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private Light2D screenLight;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource ambientSound;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject textOnBlackScreen;

    // Lights fade in
    private float innerRadiusFade;
    private float outerRadiusFade;

    // Player
    private Player p1;
    private bool gameStarted = true;
    private bool playerWait = true;

    // Camera
    CameraFollow cam;

    private void Start()
    {
        LevelManager.CUTSCENE = true;

        p1 = FindObjectOfType<Player>();
        cam = FindObjectOfType<CameraFollow>();

        // Disable screens, particles
        if (UI.activeSelf) UI.SetActive(false); // UI OFF
        if (tutorial.activeSelf) tutorial.SetActive(false); // tutorial OFF

        if (music) music.Stop();
        if (ambientSound) ambientSound.Stop();

        // Lights Start
        innerRadiusFade = 0.63f;
        outerRadiusFade = 1.2f;
        if (screenLight)
        {
            screenLight.intensity = 1f;
            screenLight.pointLightInnerRadius = innerRadiusFade;
            screenLight.pointLightOuterRadius = outerRadiusFade;
        }

        // Stops time and calls INTRO
        // Enables a black screen
        blackScreen.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(Intro());
      
    }

    void FixedUpdate()
    {
        if (playerWait) // makes player fly
            p1.Movement.Rb.gravityScale = 0f;

        if (gameStarted) // Sets light to player position
            screenLight.transform.position = p1.transform.position + new Vector3(0f, 0.3f, 0f);

        // When the player passes through a certain point
        if (cam.cameraMoving)
        {
            Destroy(textOnBlackScreen);

            gameStarted = false;

            // activates uis
            if (UI) UI.SetActive(true); // UI OFF

            // Fades in the screenLights
            if (innerRadiusFade < 3.4f) innerRadiusFade += 0.5f * Time.fixedDeltaTime;
            if (outerRadiusFade < 20.62f) outerRadiusFade += 3f * Time.fixedDeltaTime;
            screenLight.pointLightInnerRadius = innerRadiusFade;
            screenLight.pointLightOuterRadius = outerRadiusFade;
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
            if (ambientSound) ambientSound.Play();
            yield return new WaitForSeconds(2f);
            playerWait = false;
            yield return new WaitForSeconds(2f);
            tutorial.SetActive(true);
            yield return new WaitForSeconds(1f);
            break;
        }
        LevelManager.CUTSCENE = false;
    }
}
