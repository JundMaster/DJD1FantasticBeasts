using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats { get; private set; }

    [SerializeField] Transform magicPosition;
    [SerializeField] GameObject magicPrefab;

    private void Awake()
    {
        stats = new Stats();
    }

    private void Start()
    {
        stats.RangedDamage = 25f;
        stats.CanRangeAttack = false;
        stats.RangedAttackDelay = 3f;
    }

    private void Update()
    {
        if (stats.CanRangeAttack == false)
            stats.RangedAttackCounter -= Time.deltaTime;
        if (stats.RangedAttackCounter < 0)
        {   // If timeDelay gets < 0, sets timer back to AttackDelay again and the character can attack
            stats.RangedAttackCounter = stats.RangedAttackDelay;
            stats.CanRangeAttack = true;
        }

        if (stats.CanRangeAttack)
            Shoot();


        if (!(stats.IsAlive))
        {
            stats.Die(gameObject);
        }
    }

    void Shoot()
    {
        stats.CanRangeAttack = false;
        Instantiate(magicPrefab, magicPosition.position, magicPosition.rotation);
    }
}
