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
    [SerializeField] private Transform  respawn5;
    [SerializeField] private Transform  respawn6;
    [SerializeField] private GameObject respawnPrefab;

    [SerializeField] private Transform  respawnBossPosition;
    [SerializeField] private GameObject bossRespawnPrefab;
    [SerializeField] private GameObject bossRespawnAnimation;


    // newt Lives ////
    public static int   NewtLives   { get; set; } = 5;
    public static bool  AssistMode  { get; set; } = false;
    private int livesKeeper;

    // GAME STATUS
    public static bool GAMEOVER { get; set; } = false;

    private Player  p1;

    // Creatures saved
    public static byte CreaturesSaved { get; set; }

    // Boss
    public static bool  reachedBoss;
    private Boss        boss;
    [SerializeField] private GameObject finalGameBarrier;

    // Checkpoints
    private float maxPositionReached;
    private bool reachedRespawn2, reachedRespawn3, reachedRespawn4, reachedRespawn5, reachedRespawn6;

    private void Start()
    {
        Time.timeScale = 1f;

        Instantiate(player, respawn1.position, respawn1.transform.rotation);
        p1 = FindObjectOfType<Player>();

        // Checkpoint
        maxPositionReached = p1.transform.position.x;
        reachedRespawn2 = false;
        reachedRespawn3 = false;
        reachedRespawn4 = false;
        reachedRespawn5 = false;
        reachedRespawn6 = false;

        // mouse not visible
        Cursor.visible = false;
    }

    private void Update()
    {
        // Lives
        if (NewtLives < 0) NewtLives = 0;
        if (AssistMode) NewtLives = livesKeeper;
        else livesKeeper = NewtLives;

        if (p1 == null)
        {
            p1 = FindObjectOfType<Player>();
        }

        if (p1 != null)
        {
            if (p1.transform.position.x > maxPositionReached)
                maxPositionReached = p1.transform.position.x;
        }

        // RESPAWN 2
        if (maxPositionReached > respawn2.transform.position.x && reachedRespawn2 == false)
        {
            reachedRespawn2 = true;
            Instantiate(respawnPrefab, respawn2.transform.position, respawnPrefab.transform.rotation);
        }
        // RESPAWN 3
        else if (maxPositionReached > respawn3.transform.position.x && reachedRespawn3 == false)
        {
            reachedRespawn3 = true;
            Instantiate(respawnPrefab, respawn3.transform.position, respawnPrefab.transform.rotation);
        }
        // RESPAWN 4
        else if (maxPositionReached > respawn4.transform.position.x && reachedRespawn4 == false)
        {
            reachedRespawn4 = true;
            Instantiate(respawnPrefab, respawn4.transform.position, respawnPrefab.transform.rotation);  
        }
        // RESPAWN 5
        else if (maxPositionReached > respawn5.transform.position.x && reachedRespawn5 == false)
        {
            reachedRespawn5 = true;
            Instantiate(respawnPrefab, respawn5.transform.position, respawnPrefab.transform.rotation);
        }
        // RESPAWN 6
        else if (maxPositionReached > respawn6.transform.position.x && reachedRespawn6 == false)
        {
            reachedRespawn6 = true;
            Instantiate(respawnPrefab, respawn6.transform.position, respawnPrefab.transform.rotation);
        }


        // Spawns boss
        if (p1 != null)
        {
            if (p1.transform.position.x > 139.5f && reachedBoss == false && Boss.BossDefeated == false)
            {
                SpawnBoss();
            }
        }
        // If the player is fighting the boss
        if (reachedBoss)
        {
            // If the boss dies, it destroys the last barrier
            if (boss == null)
            {
                Destroy(finalGameBarrier);
            }
        }
    }

    public void SpawnBoss()
    {
        reachedBoss = true;

        // Spawns Boss
        Instantiate(bossRespawnAnimation, respawnBossPosition.position + new Vector3(0f, 0.2f, 0f), respawnBossPosition.rotation);
        Instantiate(bossRespawnPrefab, respawnBossPosition.position, respawnBossPosition.rotation);

        // Finds boss
        if (boss == null) boss = FindObjectOfType<Boss>();
    }


    public void Respawn()
    {
        // RESPAWN1
        if (respawn1)
        {
            if (maxPositionReached > respawn1.position.x)
                if (maxPositionReached < respawn2.position.x)
                    Instantiate(player, respawn1.position, transform.rotation);
        }

        // RESPAWN2
        if (respawn2)
        {
            if (maxPositionReached > respawn2.position.x)
                if (maxPositionReached < respawn3.position.x)
                    Instantiate(player, respawn2.position, transform.rotation);
        }


        // RESPAWN3
        if (respawn3)
        {
            if (maxPositionReached > respawn3.position.x)
                if (maxPositionReached < respawn4.position.x)
                    Instantiate(player, respawn3.position, transform.rotation);
        }

        // RESPAWN4
        if (respawn4)
        {
            if (maxPositionReached > respawn4.position.x)
                if (maxPositionReached < respawn5.position.x)
                    Instantiate(player, respawn4.position, transform.rotation);
        }

        // RESPAWN5
        if (respawn5)
        {
            if (maxPositionReached > respawn5.position.x)
                if (maxPositionReached < respawn6.position.x)
                    Instantiate(player, respawn5.position, transform.rotation);
        }

        // RESPAWN6 // BEFORE BOSS
        if (respawn6)
        {
            if (maxPositionReached > respawn6.position.x)
            {
                Instantiate(player, respawn6.position, transform.rotation);

                // Sets reached boss to false, if player dies
                reachedBoss = false;

                GameObject[] bossBoxSpawn = GameObject.FindGameObjectsWithTag("BossBoxSpawn");
                GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");

                foreach (GameObject box in bossBoxSpawn)
                {
                    Destroy(box);
                }
                Destroy(bossObject);
            }
        }
    }
}
