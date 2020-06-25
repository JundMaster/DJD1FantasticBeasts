using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class BossSpawner : MonoBehaviour
{

    [SerializeField] private GameObject smokeSpawner;

    public void DestroyMe()
    {
        Instantiate(smokeSpawner, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
