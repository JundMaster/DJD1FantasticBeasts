using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] GameObject     pickedUp;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.transform.GetComponent<Player>();

        if (player != null)
        {
            if (!(player.Stats.IsMaxHP()))
            {
                player.Stats.HealHP(30f);
                Instantiate(pickedUp, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
