using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] GameObject     ammunitionHit;

    [SerializeField] Rigidbody2D    rb;
    [SerializeField] float          rangedDamage; 
    [SerializeField] float          speed;


    // Start is called before the first frame update
    void Start()
    {

        speed = 4f;
        rb.velocity = transform.right * speed;

        // Destroys the object if it doesn't hit anything
        Destroy(gameObject, 400f * Time.deltaTime);

    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Treasure treasure = hitInfo.transform.GetComponent<Treasure>();
        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();


        // If there's a collision
        if (treasure != null)
        {
            treasure.stats.TakeDamage(rangedDamage);
        }

        if (enemy != null)
        {
            enemy.stats.TakeDamage(rangedDamage);
        }


        Instantiate(ammunitionHit, transform.position, transform.rotation);

        Destroy(gameObject);


    }
}
