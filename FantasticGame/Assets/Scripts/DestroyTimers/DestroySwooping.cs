using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySwooping : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] GameObject swoopingSpawnerPrefab;

    float dieCounter;


    // Only 1 swooping evil alive at a time
    public static bool swoopingIsAlive;
    // Start is called before the first frame update
    void Start()
    {
        swoopingIsAlive = true;
        player = FindObjectOfType<PlayerMovement>();
        dieCounter = 20f;
    }

    
    private void Update()
    {
        // Starts a counter as soon as swooping spawns
        if (swoopingIsAlive)
        {
            dieCounter -= Time.deltaTime;
            if (dieCounter < 0)
            {
                swoopingIsAlive = false;
            }
        }

        // Happens when player dies or swooping dies
        if (swoopingIsAlive == false)
        {
            Instantiate(swoopingSpawnerPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player jumps on it, it will push the player top
        if (collision != null)
        {
            player.rb.velocity = new Vector2(0f, 5f);
            swoopingIsAlive = false;
            Instantiate(swoopingSpawnerPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
            
    }


}
