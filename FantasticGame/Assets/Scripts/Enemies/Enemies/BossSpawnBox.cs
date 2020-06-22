using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class BossSpawnBox : MonoBehaviour
{
    // Gets spwaner prefab
    [SerializeField] private GameObject spawnprefab;

    private float random;

    // Timers
    private float descendCounter1, descendCounter2, descendCounter3;

    // Left or Right box
    private bool leftBox    = false;

    private Camera camera;

    void Start()
    {
        camera = Camera.main;

        // Spawn prefab
        Instantiate(spawnprefab, transform.position, transform.rotation);

        // Random rotation
        random = Random.Range(0, 100);

        // Which Box is this
        if (transform.position.x < 141) leftBox = true;

        // Timers
        descendCounter1 = 2f;
        descendCounter2 = 4f;
        descendCounter3 = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        BoxRotation();
        BoxMovement();
        OutOfBounds();
    }

    private void BoxMovement()
    {
        if (random < 33)
        {
            descendCounter1 -= Time.deltaTime;
            // Lowers the box
            if (descendCounter1 < 0 && transform.position.y > 0.25f)
            {
                MoveBoxDown();
            }
            // Once it gets to the bottom position, moves the box left or right
            else if (descendCounter1 < 0)
            {
                MoveBoxHorizontal();
            }
        }
        else if (random < 66)
        {
            descendCounter2 -= Time.deltaTime;
            // Lowers the box
            if (descendCounter2 < 0 && transform.position.y > 0.25f)
            {
                MoveBoxDown();
            }
            // Once it gets to the bottom position, moves the box left or right
            else if (descendCounter2 < 0)
            {
                MoveBoxHorizontal();
            }
        }
        else
        {
            descendCounter3 -= Time.deltaTime;
            // Lowers the box
            if (descendCounter3 < 0 && transform.position.y > 0.25f)
            {
                MoveBoxDown();
            }
            // Once it gets to the bottom position, moves the box left or right
            else if (descendCounter3 < 0)
            {
                MoveBoxHorizontal();
            }
        }
    }

    private void MoveBoxHorizontal()
    {
        if (leftBox)
        {
            // Different speeds depending on the random number
            if (random < 16) transform.position += new Vector3(7f * Time.deltaTime, 0f, 0f);
            else transform.position += new Vector3(4f * Time.deltaTime, 0f, 0f);
        }
        else // right box
        {
            if (random < 16) transform.position -= new Vector3(7f * Time.deltaTime, 0f, 0f);
            else transform.position -= new Vector3(4f * Time.deltaTime, 0f, 0f);
        }
    }

    private void MoveBoxDown()
    {
        transform.position -= new Vector3(0f, 1f * Time.deltaTime, 0f);
    }

    private void BoxRotation()
    {
        // Random rotation
        if (random < 50)
            transform.eulerAngles += new Vector3(0f, 0f, 100f * Time.deltaTime);
        else
            transform.eulerAngles += new Vector3(0f, 0f, -100f * Time.deltaTime);
    }

    private void OutOfBounds()
    {
        // Destroys the object if it doesn't hit anything
        if ((gameObject.transform.position.x > (camera.transform.position.x) + (camera.aspect * 2f * camera.orthographicSize)) ||
            (gameObject.transform.position.x < (camera.transform.position.x) - (camera.aspect * 2f * camera.orthographicSize)))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Gets player
        Player player = hitInfo.transform.GetComponent<Player>();

        // Does something if there's a collision with the player
        if (player != null)
        {
            // Damages player and destroys itself
            player.Stats.TakeDamage(10f);
            StartCoroutine(player.CameraShake.Shake(0.015f, 0.04f));
            Instantiate(spawnprefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
