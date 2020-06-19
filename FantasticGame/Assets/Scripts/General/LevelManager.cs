using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

sealed public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform  respawn1;
    [SerializeField] private Transform  respawn2;
    [SerializeField] private Transform  respawn3;
    [SerializeField] private Transform  respawn4;
    [SerializeField] private GameObject respawnPrefab;


    private Player  p1;

    // Checkpoints
    private float   maxPositionReached;
    private bool    reachedRespawn2, reachedRespawn3, reachedRespawn4;


    private void Start()
    {
        Instantiate(player, respawn1.position, respawn1.transform.rotation);

        p1 = FindObjectOfType<Player>();

        // Checkpoint
        maxPositionReached = p1.transform.position.x;
        reachedRespawn2 = false;
        reachedRespawn3 = false;
        reachedRespawn4 = false;
        // mouse not visible
        Cursor.visible = false;   
    }

    private void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<Player>();
        }

        if (p1.transform.position.x > maxPositionReached)
            maxPositionReached = p1.transform.position.x;


        // RESPAWN 2
        if (maxPositionReached > respawn2.transform.position.x && reachedRespawn2 == false)
        {
            Instantiate(respawnPrefab, respawn2.transform.position, respawnPrefab.transform.rotation);
            reachedRespawn2 = true;
        }
        // RESPAWN 3
        if (maxPositionReached > respawn3.transform.position.x && reachedRespawn3 == false)
        {
            Instantiate(respawnPrefab, respawn3.transform.position, respawnPrefab.transform.rotation);
            reachedRespawn3 = true;
        }
        // RESPAWN 4
        if (maxPositionReached > respawn4.transform.position.x && reachedRespawn4 == false)
        {
            Instantiate(respawnPrefab, respawn4.transform.position, respawnPrefab.transform.rotation);
            reachedRespawn4 = true;
        }
    }

    public void Respawn()
    {
        // RESPAWN1
        if (maxPositionReached > respawn1.transform.position.x)
            if (maxPositionReached < respawn2.transform.position.x)
                Instantiate(player, respawn1.position, transform.rotation);

        // RESPAWN2
        if (maxPositionReached > respawn2.transform.position.x)
            if (maxPositionReached < respawn3.transform.position.x)
                Instantiate(player, respawn2.position, transform.rotation);


        // RESPAWN3
        if (maxPositionReached > respawn3.transform.position.x)
            if (maxPositionReached < respawn4.transform.position.x)
                Instantiate(player, respawn3.position, transform.rotation);


        // RESPAWN4
        if (maxPositionReached > respawn4.transform.position.x)
            Instantiate(player, respawn4.position, transform.rotation);
        
        
    }


}
