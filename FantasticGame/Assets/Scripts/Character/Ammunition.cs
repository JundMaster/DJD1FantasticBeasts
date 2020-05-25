using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] GameObject     ammunitionHit;
    [SerializeField] Rigidbody2D    rb;
    [SerializeField] float          speed;

    Player p1;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        rb.velocity = transform.right * speed;

        camera = Camera.main;

        // Destroys the object if it doesn't hit anything
        //Destroy(gameObject, 150 * Time.deltaTime);
        p1 = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if ((gameObject.transform.position.x > (camera.transform.position.x) + (camera.aspect * 2f * camera.orthographicSize)) ||
            (gameObject.transform.position.x < (camera.transform.position.x) - (camera.aspect * 2f * camera.orthographicSize)))
                Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Treasure treasure = hitInfo.GetComponent<Treasure>();
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        EnemyMelee enemyMelee = hitInfo.GetComponent<EnemyMelee>();


        // If there's a collision
        if (treasure != null)
        {
            treasure.Stats.TakeDamage(p1.Stats.RangedDamage);
        }

        if (enemy != null)
        {
            enemy.Stats.TakeDamage(p1.Stats.RangedDamage);
        }

        if (enemyMelee != null)
        {
            enemyMelee.Stats.TakeDamage(p1.Stats.RangedDamage);
        }

        Instantiate(ammunitionHit, transform.position, transform.rotation);

        Destroy(gameObject);


    }
}
