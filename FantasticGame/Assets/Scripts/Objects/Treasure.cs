using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Stats stats { get; private set; }

    private void Awake()
    {
        stats = new Stats(100f);
    }


    private void Update()
    {
        if (!(stats.IsAlive))
        {
            stats.Die(gameObject);
        }
    }



}
