using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHP = 200;
    [SerializeField] public int damage { get; } = 50;
    int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void takeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
