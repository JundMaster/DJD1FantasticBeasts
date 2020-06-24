using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class GOBLINWINNING : EnemyBaseRanged
{
    protected override void Awake()
    {
        Destroy(gameObject, 2500f * Time.deltaTime);
    }

    protected override void Movement()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
