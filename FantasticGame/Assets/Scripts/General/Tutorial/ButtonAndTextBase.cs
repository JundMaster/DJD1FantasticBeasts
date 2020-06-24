using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAndTextBase : MonoBehaviour
{
    // Color text fadein
    protected float alpha;

    // ETC
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;

        alpha = 0;

        if (LevelManager.WONGAME == false)
        {
            SoundManager.PlaySound(AudioClips.textPopUp);
        }
    }

    private void FixedUpdate()
    {
        // Increments alpha
        if (alpha < 1)
            alpha += 0.5f * Time.fixedDeltaTime;

        // Destroys the object
        OutOfBounds();
    }

    protected void OutOfBounds()
    {
        // Destroys the object if it's off-screen
        if ((gameObject.transform.position.x > (camera.transform.position.x) + (camera.aspect * 2f * camera.orthographicSize)) ||
            (gameObject.transform.position.x < (camera.transform.position.x) - (camera.aspect * 2f * camera.orthographicSize)))
            Destroy(gameObject);
    }
}
