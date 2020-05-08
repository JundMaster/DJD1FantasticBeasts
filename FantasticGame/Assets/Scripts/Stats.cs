using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private float   maxMana;
    private float   currentMana;
    private float   attackManaCost;
    private float   manaRegen;

    private float   maxHP;
    private float   currentHP;

    private bool    isAlive = true;

    private float   rangedDamage;
    private bool    canRangeAttack;
    private bool    rangedAtacking;
    private float   rangedAttackDelay;
    private float   rangedAttackCounter;
    
    private float   meleeDamage;
    private bool    canMeleeAttack;
    private float    meleeAttackRange;
    private float   meleeAttackDelay;
    private float   meleeAttackCounter;



    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            isAlive = false;
    }


    public void Die()
    {
        Destroy(gameObject);
    }


    public bool CanUseSpell()
    {
        bool useSpell = false;
        if (currentMana - attackManaCost > 0) useSpell = true;
        return useSpell;
    }


    public void RegenMana()
    {
        if (currentMana < maxMana)
            currentMana += Time.deltaTime * manaRegen;
    }


    public void SpendMana()
    {
        currentMana -= attackManaCost;
    }


    public float MaxMana
    {
        get
        {
            return maxMana;
        }
        set
        {
            maxMana = value;
        }
    }

    public float CurrentMana
    {
        get
        {
            return currentMana;
        }
        set
        {
            currentMana = value;
        }
    }

    public float AttackManaCost
    {
        get
        {
            return attackManaCost;
        }
        set
        {
            attackManaCost = value;
        }
    }

    public float ManaRegen
    {
        get
        {
            return manaRegen;
        }
        set
        {
            manaRegen = value;
        }
    }

    public float MaxHP
    {
        get
        {
            return maxHP;
        }
        set
        {
            maxHP = value;
        }
    }

    public float CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

    public float RangedDamage
    {
        get
        {
            return rangedDamage;
        }
        set
        {
            rangedDamage = value;
        }
    }


    public bool CanRangeAttack
    {
        get
        {
            return canRangeAttack;
        }
        set
        {
            canRangeAttack = value;
        }
    }

    public bool RangedAttacking
    {
        get
        {
            return rangedAtacking;
        }
        set
        {
            rangedAtacking = value;
        }
    }

    public float RangedAttackDelay
    {
        get
        {
            return rangedAttackDelay;
        }
        set
        {
            rangedAttackDelay = value;
        }
    }


    public float RangedAttackCounter
    {
        get
        {
            return rangedAttackCounter;
        }
        set
        {
            rangedAttackCounter = value;
        }
    }


    public float MeleeDamage
    {
        get
        {
            return meleeDamage;
        }
        set
        {
            meleeDamage = value;
        }
    }


    public bool CanMeleeAttack
    {
        get
        {
            return canMeleeAttack;
        }
        set
        {
            canMeleeAttack = value;
        }
    }


    public float MeleeAttackRange
    {
        get
        {
            return meleeAttackRange;
        }
        set
        {
            meleeAttackRange = value;
        }
    }


    public float MeleeAttackDelay
    {
        get
        {
            return meleeAttackDelay;
        }
        set
        {
            meleeAttackDelay = value;
        }
    }

    public float MeleeAttackCounter
    {
        get
        {
            return meleeAttackCounter;
        }
        set
        {
            meleeAttackCounter = value;
        }
    }
}







