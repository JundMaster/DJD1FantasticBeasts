using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseTrack : MonoBehaviour
{
    Camera mainCamera;

    [SerializeField] GameObject     particleMousePrefab;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePositionPixels = Input.mousePosition;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(mousePositionPixels);


        if (Cursor.visible)
        {
            Instantiate(particleMousePrefab, mousePosition, particleMousePrefab.transform.rotation);
        }
    }
}
