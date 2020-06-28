using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

sealed public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] respawnPositions;
    [SerializeField] private GameObject respawnPrefab;

    [SerializeField] private Transform  respawnBossPosition;
    [SerializeField] private GameObject bossRespawnPrefab;
    [SerializeField] private GameObject bossRespawnAnimation;
    [SerializeField] private float      reachedBossPosition;

    private Player p1;

    // newt Lives ////
    public static int   NewtLives   { get; set; } = 3;
    public static bool  AssistMode  { get; set; } = false;
    private int         livesKeeper;

    // GAME STATUS
    public static bool GAMEOVER { get; set; } = false;
    public static bool WONGAME  { get; set; } = false;
    public static bool CUTSCENE { get; set; } = false;

    // Creatures saved
    public static byte CreaturesSaved   { get; set; } = 0;

    // Boss
    public static bool  ReachedBoss     { get; set; } = false;
    public static bool  BossDefeated    { get; set; } = false;
    private Boss        boss;
    private Boss_02     boss_02;
    [SerializeField] private GameObject finalGameBarrier;

    // Checkpoints
    private float   maxPositionReached;
    private bool[]  reachedRespawns;

    private void Awake()
    {
        Time.timeScale = 1f;

        Instantiate(player, respawnPositions[0].position, respawnPositions[0].transform.rotation);
        p1 = FindObjectOfType<Player>();

        // Checkpoints reached
        reachedRespawns = new bool[6];
        maxPositionReached = p1.transform.position.x;
        for (int i = 0; i < reachedRespawns.Length; i++)
        {
            reachedRespawns[i] = false;
        }
        reachedRespawns[0] = true;

        // mouse not visible
        Cursor.visible = false;
    }

    private void Update()
    {
        // Lives
        if (AssistMode) NewtLives = livesKeeper; // Always keeps the same lives
        else livesKeeper = NewtLives;


        // Winning condition
        if (p1 != null) if (p1.transform.position.x > CameraFollow.WinningRange) WONGAME = true;

        // Gameover condition // Turns Gameover status = true
        if (NewtLives < 1 && AssistMode == false) GAMEOVER = true;
        else GAMEOVER = false;
        

        if (p1 == null)
        {
            p1 = FindObjectOfType<Player>();
        }

        if (p1 != null)
        {
            if (p1.transform.position.x > maxPositionReached)
                maxPositionReached = p1.transform.position.x;
        }

        // RESPAWNS POSITIONS /////////////////////////////////////////////////////////////////////////
        for (int i = 1; i < respawnPositions.Length; i++)
        {
            if (maxPositionReached > respawnPositions[i].transform.position.x && reachedRespawns[i] == false)
            {
                reachedRespawns[i] = true;
                Instantiate(respawnPrefab, respawnPositions[i].transform.position, respawnPrefab.transform.rotation);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        // Spawns boss
        if (p1 != null)
        {
            if (p1.transform.position.x > reachedBossPosition && ReachedBoss == false && BossDefeated == false)
            {
                SpawnBoss();
            }
        }
        // If the player is fighting the boss
        if (ReachedBoss)
        {
            // If the boss dies, it destroys the last barrier
            if (boss == null && boss_02 == null)
            {
                Destroy(finalGameBarrier);
            }
        }
    }

    public void SpawnBoss()
    {
        ReachedBoss = true;

        // Spawns Boss
        Instantiate(bossRespawnAnimation, respawnBossPosition.position + new Vector3(0f, 0.2f, 0f), respawnBossPosition.rotation);
        Instantiate(bossRespawnPrefab, respawnBossPosition.position, respawnBossPosition.rotation);

        // Finds boss
        if (boss == null) boss = FindObjectOfType<Boss>();
        if (boss_02 == null) boss_02 = FindObjectOfType<Boss_02>();
    }


    // Respawns depending on the position reached
    public void Respawn()
    {
        // Destroys every pickup everytime the player respawns
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("pickUp");
        foreach (GameObject pickUp in pickUps)
            Destroy(pickUp);

        // Respawns the player depending on the max position reached
        for (int i = 0; i < respawnPositions.Length; i++)
        {
            if (i < 5)
            {
                if (respawnPositions[i])
                {
                    if (reachedRespawns[i])
                    {
                        if (maxPositionReached < respawnPositions[i + 1].position.x)
                        {
                            Instantiate(player, respawnPositions[i].position, transform.rotation);
                        }
                    }
                }
            }
            // for respawn 6 ( boss respawn )
            else
            {
                if (respawnPositions[i])
                {
                    if (reachedRespawns[i])
                    {
                        Instantiate(player, respawnPositions[i].position, transform.rotation);

                        // Sets reached boss to false, if player dies
                        ReachedBoss = false;

                        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
                        GameObject[] bossBoxSpawn = GameObject.FindGameObjectsWithTag("BossBoxSpawn");

                        // Level 02
                        if (SceneManager.GetActiveScene().name == "Level02")
                        {
                            GameObject mainScreenLight = GameObject.FindGameObjectWithTag("screenMainLight");
                            Light2D screenLight = mainScreenLight.GetComponent<Light2D>();
                            screenLight.pointLightOuterRadius = 20.62f;
                            screenLight.pointLightInnerRadius = 3.4f;
                            screenLight.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
                        }

                        // Both levels // Level 1 = boxes // Level 2 = particles
                        foreach (GameObject box in bossBoxSpawn)
                        {
                            Destroy(box);
                        }

                        Destroy(bossObject);
                    }
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 255);
        Gizmos.DrawLine(new Vector3(reachedBossPosition, -30f, 0f), new Vector3(reachedBossPosition, 30f, 0f));
    }
}
