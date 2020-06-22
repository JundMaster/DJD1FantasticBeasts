using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Niffler : PowerUpBase
{
    public Niffler()
    {
        base.Type = PowerUpType.niffler;
    }

    protected override void PickUpAbility(Player player)
    {
        LevelManager.creaturesSaved++;
        PickAndDestroy();
    }

    protected override void PickAndDestroy()
    {
        SoundManager.PlaySound(AudioClips.niffler);
        Instantiate(pickedUp, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);
        Destroy(gameObject);
    }
}
