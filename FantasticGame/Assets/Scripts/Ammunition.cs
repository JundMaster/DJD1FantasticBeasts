using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] GameObject hittingSprite;

    [SerializeField] Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        //Enemy enemy = hitSomething.transform.GetComponent<Enemy>();
        //if (enemy != null)
        //{
        //    enemy.takeDamage(damage);
        //}

        //Instantiate(hittingSprice, transform.position, transform.rotation);
        Destroy(gameObject);
        
    }
}
