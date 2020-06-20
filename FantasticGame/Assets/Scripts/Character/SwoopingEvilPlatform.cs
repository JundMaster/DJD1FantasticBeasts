using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SwoopingEvilPlatform : MonoBehaviour
{
    [SerializeField] private GameObject swoopingSpawnerPrefab;

    private PlayerMovement player;

    private float dieCounter;

    // Only 1 swooping evil alive at a time
    public static bool isAlive;

    private Camera camera;

    void Start()
    {
        isAlive = true;
        player = FindObjectOfType<PlayerMovement>();
        dieCounter = 20f;
        camera = Camera.main;

        if (SwoopingEvilPlatform.isAlive)
        {
            StartCoroutine(swoopingSound());
        }
        else if (!SwoopingEvilPlatform.isAlive)
        {
            StopCoroutine(swoopingSound());
        }
    }

    private void Update()
    {
        // Starts a counter as soon as swooping spawns
        if (isAlive)
        {
            dieCounter -= Time.deltaTime;
            if (dieCounter < 0)
            {
                isAlive = false;
            }
        }

        // Happens when player dies or swooping dies
        if (isAlive == false)
        {
            Instantiate(swoopingSpawnerPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    // Plays swooping platform sound (if swooping is inside the screen)
    IEnumerator swoopingSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if ((gameObject.transform.position.x < (camera.transform.position.x) + (camera.aspect * camera.orthographicSize)) &&
                (gameObject.transform.position.x > (camera.transform.position.x) - (camera.aspect * camera.orthographicSize)))
            {
                SoundManager.PlaySound(AudioClips.swoopingPlatform); // plays sound
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player jumps on it, it will push the player top
        if (collision.gameObject.GetComponent<Player>())
        {
            player.Rb.velocity = new Vector2(0f, 5f);
            isAlive = false;
            Instantiate(swoopingSpawnerPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }     
    }


}
