using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

sealed public class Text : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    // Color text fadein
    private float alpha = 0;

    // ETC
    private Player p1;
    private Camera camera;

    void Awake()
    {
        p1 = FindObjectOfType<Player>();
        camera = Camera.main;

        if (text) text.color = new Color(1, 1, 1, alpha);

        if (LevelManager.WONGAME == false)
        {
            SoundManager.PlaySound(AudioClips.textPopUp);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (p1 == null)
            p1 = FindObjectOfType<Player>();

        if (alpha < 1)
            alpha += 0.5f * Time.fixedDeltaTime;

        if (text) text.color = new Color(1, 1, 1, alpha);

        // Destroys the object
        OutOfBounds();
    }

    private void OutOfBounds()
    {
        // Destroys the object if it's off-screen
        if ((gameObject.transform.position.x > (camera.transform.position.x) + (camera.aspect * 2f * camera.orthographicSize)) ||
            (gameObject.transform.position.x < (camera.transform.position.x) - (camera.aspect * 2f * camera.orthographicSize)))
            Destroy(gameObject);
    }
}
