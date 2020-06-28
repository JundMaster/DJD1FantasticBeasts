using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Treasure : MonoBehaviour
{
    // Drops - gameobjects
    [SerializeField] private GameObject healthPickUp, manaPickUp;

    // Stats property
    public Stats Stats { get; private set; }

    private void Start()
    {
        // Initialises stats
        Stats = new Stats
        {
            IsAlive     = true,
            CurrentHP   = 25f
        };
    }

    private void Update()
    {
        if (!Stats.IsAlive)
        {
            int chance = Random.Range(0, 10);
            // Has a 100 chance of spawning 50/50 mana or health
            if (healthPickUp != null && chance >= 5)
            {
                Instantiate(healthPickUp, transform.position, transform.rotation);
            }
            else if (manaPickUp != null)
            {
                Instantiate(manaPickUp, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }



}
