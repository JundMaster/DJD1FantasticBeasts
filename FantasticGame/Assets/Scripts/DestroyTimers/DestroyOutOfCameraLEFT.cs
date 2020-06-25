using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class DestroyOutOfCameraLEFT : MonoBehaviour
{
    // ETC
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        // Destroys the object
        OutOfBounds();
    }

    private void OutOfBounds()
    {
        // Destroys the object if it's off-screen
        if (gameObject.transform.position.x < camera.transform.position.x - camera.aspect * 2f * camera.orthographicSize)
            Destroy(gameObject);
    }
}
