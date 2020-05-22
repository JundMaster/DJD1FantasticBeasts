using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform  respawnPoint;


    void Awake()
    {
        Instantiate(player, respawnPoint.position, transform.rotation);
    }


    public void Respawn()
    {
        Instantiate(player, respawnPoint.position, transform.rotation);
    }


}
