using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class HealthPickUp : PowerUpBase
{
    public HealthPickUp()
    {
        base.Type = PowerUpType.health;
    }

    protected override void PickUpAbility(Player player)
    {
        if (!(player.Stats.IsMaxHP()))
        {
            player.Stats.HealHP(30f);
            PickAndDestroy();
        }
    }
}
