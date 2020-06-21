using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class ManaPickUp : PowerUpBase
{
    public ManaPickUp()
    {
        base.Type = PowerUpType.mana;
    }

    protected override void PickUpAbility(Player player)
    {
        if (!(player.Stats.IsMaxMana()))
        {
            player.Stats.HealMana(30f);
            PickAndDestroy();
        }
    }
}
