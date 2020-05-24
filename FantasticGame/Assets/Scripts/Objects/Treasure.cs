using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    // Drops
    [SerializeField] GameObject     healthPickUp, manaPickUp;


    public Stats Stats { get; private set; }

    private void Awake()
    {
        Stats = new Stats();
    }

    private void Start()
    {
        Stats.IsAlive = true;
        Stats.CurrentHP = 25f;
    }

    private void Update()
    {
        if (!Stats.IsAlive)
        {
            int chance = Random.Range(0, 10);
            if (healthPickUp != null && chance >= 5) Instantiate(healthPickUp, transform.position, transform.rotation);
            else if (manaPickUp != null) Instantiate(manaPickUp, transform.position, transform.rotation);


            Stats.Die(gameObject);
        }
    }



}
