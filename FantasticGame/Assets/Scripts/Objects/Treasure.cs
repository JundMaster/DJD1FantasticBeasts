using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    // Drops
    [SerializeField] GameObject     healthPickUp, manaPickUp;


    public Stats stats { get; private set; }

    private void Awake()
    {
        stats = new Stats();
    }

    private void Start()
    {
        stats.CurrentHP = 100f;
    }

    private void Update()
    {
        if (!(stats.IsAlive))
        {
            int chance = Random.Range(0, 10);
            if (healthPickUp != null && chance >= 5) Instantiate(healthPickUp, transform.position, transform.rotation);
            else if (manaPickUp != null) Instantiate(manaPickUp, transform.position, transform.rotation);


            stats.Die(gameObject);
        }
    }



}
