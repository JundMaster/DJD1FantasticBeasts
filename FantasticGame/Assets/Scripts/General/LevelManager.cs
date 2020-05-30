using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform  respawn1;
    [SerializeField] Transform  respawn2;
    [SerializeField] Transform  respawn3;
    [SerializeField] Transform  respawn4;
    [SerializeField] GameObject respawnPrefab;




    Player p1;
    float   maxPositionReached;
    bool    reachedRespawn2, reachedRespawn3, reachedRespawn4;


    void Awake()
    {
        Instantiate(player, respawn1.position, transform.rotation);
        p1 = FindObjectOfType<Player>();
    }

    private void Start()
    {
        maxPositionReached = p1.transform.position.x;
        reachedRespawn2 = false;
        reachedRespawn3 = false;
        reachedRespawn4 = false;
        Cursor.visible = false;
    }

    private void Update()
    {

        p1 = FindObjectOfType<Player>();

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



        //if (Input.GetKeyDown("r"))
        //    SceneManager.LoadScene(1);
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
