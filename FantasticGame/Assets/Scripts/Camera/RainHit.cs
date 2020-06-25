using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class RainHit : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }


    void Update()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }



        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);


        Vector3 camBottom = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        if (camera != null)
        {
            if (transform.position.y < camBottom.y)
                particle.Pause();
            else
                particle.Play();
        }
    }
}
