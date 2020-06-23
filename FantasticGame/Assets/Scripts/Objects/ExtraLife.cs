using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class ExtraLife : PowerUpBase
{
    public ExtraLife()
    {
        base.Type = PowerUpType.mana;
    }

    protected override void PickUpAbility(Player player)
    {
        LevelManager.NewtLives++;
        base.PickAndDestroy();
    }
}
