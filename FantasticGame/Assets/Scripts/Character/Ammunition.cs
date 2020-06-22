using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Ammunition : MonoBehaviour
{
    // Editor stuff
    [SerializeField] private GameObject     ammunitionHit;
    [SerializeField] private Rigidbody2D    rb;
    [SerializeField] private float          speed;

    private Player p1;
    private Camera camera;

    void Start()
    {
        // Sets initial speed
        speed = 4f;
        rb.velocity = transform.right * speed;

        camera  = Camera.main;
        p1      = FindObjectOfType<Player>();
    }

    private void Update()
    {
        // Destroys the object if it doesn't hit anything
        if ((gameObject.transform.position.x > (camera.transform.position.x) + (camera.aspect * 2f * camera.orthographicSize)) ||
            (gameObject.transform.position.x < (camera.transform.position.x) - (camera.aspect * 2f * camera.orthographicSize)))
                Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Treasure treasure = hitInfo.GetComponent<Treasure>();
        SurpriseBox surpriseBox = hitInfo.GetComponent<SurpriseBox>();
        EnemyBase enemy = hitInfo.GetComponent<EnemyBase>();

        // If there's a collision
        if (treasure != null)
        {
            treasure.Stats.TakeDamage(p1.Stats.RangedDamage);
        }

        if (surpriseBox != null)
        {
            surpriseBox.Stats.TakeDamage(p1.Stats.RangedDamage);
        }

        if (enemy != null)
        {
            enemy.Stats.TakeDamage(p1.Stats.RangedDamage);
        }


        SoundManager.PlaySound(AudioClips.enemyHit); // plays sound
        // Instantiates collision prefab and destroys this gameobject
        Instantiate(ammunitionHit, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
