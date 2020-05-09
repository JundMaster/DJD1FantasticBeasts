using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats { get; private set; }

    private void Awake()
    {
        stats = new Stats(150f);
    }


    private void Update()
    {
        if (!(stats.IsAlive))
        {
            stats.Die(gameObject);
        }
    }
}
