using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmunition : MonoBehaviour
{
    [SerializeField] GameObject ammunitionHit;
    [SerializeField] GameObject ammunitionHitShield;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;

    public Enemy enemy;
    public Goblin goblinDamage;
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
        Player player = hitInfo.transform.GetComponent<Player>();

        if (player != null)
        {
            if (player.Movement.Invulnerable == false)
            {
                if (player.UsingShield)
                {
                    if (player.transform.right.x < 0) // Turned left
                    {
                        if (player.transform.position.x > rb.transform.position.x)
                        {
                            Instantiate(ammunitionHitShield, player.ShieldPosition, transform.rotation);
                        }
                        else if (player.transform.position.x < rb.transform.position.x)
                        {
                            player.Stats.TakeDamage(enemy.Damage);
                            Instantiate(ammunitionHit, transform.position, transform.rotation);
                        }
                    }
                    else if (player.transform.right.x > 0) // Turned Right
                    {
                        if (player.transform.position.x > rb.transform.position.x)
                        {
                            Instantiate(ammunitionHit, transform.position, transform.rotation);
                            player.Stats.TakeDamage(enemy.Damage);
                        }
                        else if (player.transform.position.x < rb.transform.position.x)
                        {
                            Instantiate(ammunitionHitShield, player.ShieldPosition, transform.rotation);
                        }
                    }
                }
                else
                {
                    player.Stats.TakeDamage(enemy.Damage);
                    Instantiate(ammunitionHit, transform.position, transform.rotation);

                    if (player.transform.position.x > rb.transform.position.x)
                        player.Movement.Rb.AddForce(new Vector2(enemy.PushForce, 0f));
                    else if (player.transform.position.x < rb.transform.position.x)
                        player.Movement.Rb.AddForce(new Vector2(-enemy.PushForce, 0f));

                    StartCoroutine(player.CameraShake.Shake(0.015f, 0.04f));
                }
            }
        }
        else Instantiate(ammunitionHit, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
