using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SurpriseBox : PowerUpBase
{
    [SerializeField] private GameObject enemyInside;

    public Stats Stats { get; private set; }

    // Gets enemy canvas
    private GameObject enemyCanvas;

    public SurpriseBox()
    {
        base.Type = PowerUpType.surpriseBox;

        Stats = new Stats
        {
            IsAlive = true,
            CurrentHP = 1f
        };
    }

    private void Start()
    {
        enemyCanvas = GameObject.FindGameObjectWithTag("spawnEnemyPrefab");
    }

    private void Update()
    {
        if (!Stats.IsAlive)
        {
            Player p1 = FindObjectOfType<Player>();
            PickUpAbility(p1);
        }

        if (enemyCanvas == null)
            enemyCanvas = GameObject.FindGameObjectWithTag("spawnEnemyPrefab");
    }

    protected override void PickUpAbility(Player player)
    {
        if (enemyCanvas != null)
        {
            if (enemyInside)
            {
                // Spawns the enemy in enemy canvas, so it can print its health bar
                GameObject spawn = Instantiate(enemyInside, transform.position - new Vector3(0f, 0.15f, 0f), transform.rotation);
                spawn.transform.SetParent(enemyCanvas.transform);
            }
            PickAndDestroy();
        }
    }

    protected override void PickAndDestroy()
    {
        SoundManager.PlaySound(AudioClips.enemyHit);
        Instantiate(pickedUp, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
