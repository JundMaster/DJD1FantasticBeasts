using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Parallax : MonoBehaviour
{
    private float length;
    private float startingPos;

    // Camera
    [SerializeField] private GameObject cam;

    // Intensity
    [SerializeField] private float parallaxForce;


    private void Start()
    {
        startingPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        float temp = cam.transform.position.x * (1 - parallaxForce);

        float distance = cam.transform.position.x * parallaxForce;

        transform.position = new Vector3(startingPos + distance, 2.1f, 10);



        if (temp > startingPos + length) startingPos += length;
        else if (temp < startingPos - length) startingPos -= length;
        
    }
}
