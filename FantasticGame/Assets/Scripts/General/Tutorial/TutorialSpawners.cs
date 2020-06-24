using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class TutorialSpawners : MonoBehaviour
{
    // Spawner related stuff
    [SerializeField] private Text[] spawner;
    [SerializeField] private Button[] buttonSpawner;
    [SerializeField] private Transform[] textSpawnPositions;
    [SerializeField] private Transform[] buttonSpawnerPositions;
    [SerializeField] private Transform[] spawnInitials;

    // Spawn times
    private int spawnedTimes;

    // ETC
    private Player p1;

    void Start()
    {
        p1 = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (p1 == null)
            p1 = FindObjectOfType<Player>();

        // Spawns every tutorial text, everytime the player passes through a tutorial position
        for (int i = 0; i < spawner.Length; i++)
        {
            if (p1.transform.position.x > spawnInitials[i].position.x)
            {
                if (spawnedTimes == i)
                {
                    Instantiate(spawner[i], textSpawnPositions[i].position, transform.rotation);
                    Instantiate(buttonSpawner[i], buttonSpawnerPositions[i].position, transform.rotation);
                    spawnedTimes++;
                }
            }
        }
    }
}
