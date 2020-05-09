using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] GameObject ammunitionHit;
    [SerializeField] GameObject ammunitionHitShield;
    [SerializeField] Rigidbody2D rb;


    private float speed;
    private float rangedDamage;

    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        rangedDamage = 25f;
        rb.velocity = transform.right * speed;

        // Destroys the object if it doesn't hit anything
        Destroy(gameObject, 500f * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Treasure treasure = hitInfo.transform.GetComponent<Treasure>();
        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
        Player player = hitInfo.transform.GetComponent<Player>();

        // If there's a collision
        if (treasure != null)
        {
            treasure.stats.TakeDamage(rangedDamage);
            Instantiate(ammunitionHit, transform.position, transform.rotation);
        }

        if (enemy != null)
        {
            enemy.stats.TakeDamage(rangedDamage);
            Instantiate(ammunitionHit, transform.position, transform.rotation);
        }

        if (player != null)
        {

            if (player.usingShield)
                Instantiate(ammunitionHitShield, player.ShieldPosition, transform.rotation);
            else
            {
                
                Instantiate(ammunitionHit, transform.position, transform.rotation);
            }
        }
        
        else
            Instantiate(ammunitionHit, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
