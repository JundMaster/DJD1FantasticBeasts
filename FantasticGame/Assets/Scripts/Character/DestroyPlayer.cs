using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Destroys everything related to the player
            if (player.Stats.IsAlive == false)
            {
                // Destroys swooping evil
                SwoopingEvilPlatform.IsAlive = false; 
                // Respawns on the nearest active respawn
                player.Manager.Respawn();
                Destroy(gameObject);
            }
        }
    }
}
