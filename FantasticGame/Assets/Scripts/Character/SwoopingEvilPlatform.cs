using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopingEvilPlatform : MonoBehaviour
{
    PlayerMovement              player;
    [SerializeField] GameObject swoopingSpawnerPrefab;

    float dieCounter;


    // Only 1 swooping evil alive at a time
    public static bool isAlive;
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        player = FindObjectOfType<PlayerMovement>();
        dieCounter = 20f;
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
