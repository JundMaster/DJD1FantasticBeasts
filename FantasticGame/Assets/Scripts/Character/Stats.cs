using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float    MaxMana              { get; set; }
    public float    CurrentMana          { get; set; }
    public float    AttackManaCost       { get; set; }
    public float    ManaRegen            { get; set; }

    public float    MaxHP                { get; set; }
    public float    CurrentHP            { get; set; }

    public bool     IsAlive              { get; private set; } = true;

    public float    RangedDamage         { get; set; }
    public bool     CanRangeAttack       { get; set; }
    public bool     RangedAttacking      { get; set; }
    public float    RangedAttackDelay    { get; set; }
    public float    RangedAttackCounter  { get; set; }

    public float    MeleeDamage          { get; set; } 
    public bool     CanMeleeAttack       { get; set; }
    public float    MeleeAttackRange     { get; set; }
    public float    MeleeAttackDelay     { get; set; }
    public float    MeleeAttackCounter   { get; set; }


    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        Debug.Log(CurrentHP);
        if (CurrentHP <= 0)
            IsAlive = false;
    }


    public void Die(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public bool IsMaxHP()
    {
        bool isMaxHP = false;
        if (CurrentHP >= MaxHP)
            isMaxHP = true;
        return isMaxHP;
    }

    public void HealHP(float heal)
    {
        if (CurrentHP + heal > MaxHP)
            CurrentHP = MaxHP;
        else
            CurrentHP += heal;
    }


    public bool CanUseSpell()
    {
        bool useSpell = false;
        if (CurrentMana - AttackManaCost > 0) useSpell = true;
        return useSpell;
    }


    public void RegenMana()
    {
        if (CurrentMana < MaxMana)
            CurrentMana += Time.deltaTime * ManaRegen;
    }


    public void SpendMana()
    {
        CurrentMana -= AttackManaCost;
    }
}







