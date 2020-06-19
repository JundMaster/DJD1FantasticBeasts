using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    // Inspector, gameobject to instantiate after pick up
    [SerializeField] private GameObject pickedUp;

    // PowerUpType
    protected PowerUpType Type { get; set; }

    // What does it do
    protected abstract void PickUpAbility(Player player);

    // Action on trigger enter
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Gets player
        Player player = hitInfo.transform.GetComponent<Player>();

        // Does something if there's a collision with the player
        if (player != null)
        {
            // Does something different depending on the type of pick up
            switch (Type)
            {
                case PowerUpType.mana:
                    PickUpAbility(player);
                    break;

                case PowerUpType.health:
                    PickUpAbility(player);
                    break;
            }
        }
    }

    // Initiates the pickup gameobject and destroys this gameobject
    protected void PickAndDestroy()
    {
        Instantiate(pickedUp, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
