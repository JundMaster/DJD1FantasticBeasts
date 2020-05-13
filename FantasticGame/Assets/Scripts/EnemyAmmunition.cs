using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmunition : MonoBehaviour
{
    [SerializeField] GameObject ammunitionHit;
    [SerializeField] GameObject ammunitionHitShield;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float rangedDamage;
    [SerializeField] float speed;



    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        rb.velocity = transform.right * speed;

        // Destroys the object if it doesn't hit anything
        Destroy(gameObject, 215f * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.transform.GetComponent<Player>();

        if (player != null)
        {
            if (player.usingShield)
            {
                if (player.transform.right.x < 0) // Turned left
                {
                    if (player.transform.position.x > rb.transform.position.x)
                    {
                        Instantiate(ammunitionHitShield, player.ShieldPosition, transform.rotation);
                    }
                    else if (player.transform.position.x < rb.transform.position.x)
                    {
                        player.stats.TakeDamage(rangedDamage);
                        Instantiate(ammunitionHit, transform.position, transform.rotation);
                    }
                }
                else if (player.transform.right.x > 0) // Turned Right
                {
                    if (player.transform.position.x > rb.transform.position.x)
                    {
                        Instantiate(ammunitionHit, transform.position, transform.rotation);
                        player.stats.TakeDamage(rangedDamage);
                    }
                    else if (player.transform.position.x < rb.transform.position.x)
                    {
                        Instantiate(ammunitionHitShield, player.ShieldPosition, transform.rotation);
                    }
                }
            }
            else
            {
                player.stats.TakeDamage(rangedDamage);
                Instantiate(ammunitionHit, transform.position, transform.rotation);
            }
        }

        else Instantiate(ammunitionHit, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
