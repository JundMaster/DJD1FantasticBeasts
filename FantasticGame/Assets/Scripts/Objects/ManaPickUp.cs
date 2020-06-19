using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class ManaPickUp : PowerUpBase
{
    protected override void PickUpAbility(Player player)
    {
        if (!(player.Stats.IsMaxMana()))
        {
            player.Stats.HealMana(30f);
            PickAndDestroy();
        }
    }

    public ManaPickUp()
    {
        base.Type = PowerUpType.mana;
    }
}
