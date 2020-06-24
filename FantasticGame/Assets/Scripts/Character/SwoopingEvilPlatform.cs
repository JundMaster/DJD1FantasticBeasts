using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SwoopingEvilPlatform : MonoBehaviour
{
    [SerializeField] private GameObject swoopingSpawnerPrefab;

    private PlayerMovement player;

    private float dieCounter;

    // Only 1 swooping evil alive at a time
    public static bool IsAlive { get; set; }

    private Camera camera;

    void Start()
    {
        IsAlive = true;
        player = FindObjectOfType<PlayerMovement>();
        dieCounter = 20f;
        camera = Camera.main;

        if (IsAlive)
        {
            StartCoroutine(swoopingSound());
        }
        else if (!IsAlive)
        {
            StopCoroutine(swoopingSound());
        }
    }

    private void Update()
    {
        // Starts a counter as soon as swooping spawns
        if (IsAlive)
        {
            dieCounter -= Time.deltaTime;
            if (dieCounter < 0)
            {
                IsAlive = false;
            }
        }

        // Happens when player dies or swooping dies
        if (IsAlive == false)
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
            IsAlive = false;
            SoundManager.PlaySound(AudioClips.hit); // plays sound
            Instantiate(swoopingSpawnerPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }     
    }


}
