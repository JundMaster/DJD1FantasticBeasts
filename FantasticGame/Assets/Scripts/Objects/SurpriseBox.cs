using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class SurpriseBox : PowerUpBase
{
    [SerializeField] private GameObject enemyInside;

    public Stats Stats { get; private set; }

    public SurpriseBox()
    {
        base.Type = PowerUpType.surpriseBox;

        Stats = new Stats
        {
            IsAlive = true,
            CurrentHP = 1f
        };
    }

    private void Update()
    {
        if (!Stats.IsAlive)
        {
            Player p1 = FindObjectOfType<Player>();
            PickUpAbility(p1);
        }
    }

    protected override void PickUpAbility(Player player)
    {
        if (enemyInside)
        {
            Instantiate(enemyInside, transform.position - new Vector3(0f, 0.15f, 0f), transform.rotation);
        }
        PickAndDestroy();
    }

    protected override void PickAndDestroy()
    {
        SoundManager.PlaySound(AudioClips.enemyHit);
        Instantiate(pickedUp, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
