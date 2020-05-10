using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickUp : MonoBehaviour
{
    [SerializeField] GameObject pickedUp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.transform.GetComponent<Player>();

        if (player != null)
        {
            if (!(player.stats.IsMaxMana()))
            {
                player.stats.HealMana(30f);
                Instantiate(pickedUp, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
