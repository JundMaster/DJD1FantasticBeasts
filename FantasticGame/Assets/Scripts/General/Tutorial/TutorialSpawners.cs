using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class TutorialSpawners : MonoBehaviour
{
    // Spawner related stuff
    [SerializeField] private Text[] spawner;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform[] spawnInitials;

    // Spawn times
    public int spawnedTimes;

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
                    Instantiate(spawner[i], spawnPositions[i].position, transform.rotation);
                    spawnedTimes++;
                }
            }
        }
    }
}
