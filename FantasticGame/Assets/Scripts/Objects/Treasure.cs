using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] GameObject     healthPickUp;
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
            if (healthPickUp != null)
                Instantiate(healthPickUp, transform.position, transform.rotation);
            stats.Die(gameObject);
        }
    }



}
