using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class BossPower : MonoBehaviour
{
    // Drops - gameobjects
    [SerializeField] private GameObject healthPickUp, manaPickUp;
    [SerializeField] private float lootChance;

    // This particle system
    private bool canSpawnLoot;
    private float chance;

    // Player
    private Player p1;

    void Start()
    {
        p1 = FindObjectOfType<Player>();

        // Can spawn loot on hit and gives a random chance to spawn it
        canSpawnLoot = true;
        chance = Random.Range(0, 100);
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Player>())
        {

            // Plays hit sound once it hits the player
            if (p1.Movement.Invulnerable == false)
            {
                SoundManager.PlaySound(AudioClips.hit);
            }
            // Player gets invulnerable and takes 30 damage
            p1.Movement.Invulnerable = true;
        }

        // Has a percentage to spawn loot on the first hit
        // Drops loot on hit
        if (canSpawnLoot)
        {
            if (chance < lootChance)
            {
                if (healthPickUp != null && chance >= lootChance / 2f)
                {
                    Instantiate(healthPickUp, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                }
                else if (manaPickUp != null && chance < lootChance / 2f)
                {
                    Instantiate(manaPickUp, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                }
            }
            canSpawnLoot = false;
        }
    }
}
