using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] GameObject ammunitionHit;

    [SerializeField] Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        // Destroys the object if it doesn't hit anything
        Destroy(gameObject, 250f*Time.deltaTime);
    }

    // Checks for a collision
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);

        Treasure treasure = hitInfo.transform.GetComponent<Treasure>();

        // If there's a collision with a treasure
        if (treasure != null)
        {
            treasure.takeDamage(damage);
        }


        // Instantiates hit animation and destroys this object
        Instantiate(ammunitionHit, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
