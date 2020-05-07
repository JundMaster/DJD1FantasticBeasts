using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] float maxMana = 100f;
    [SerializeField] float currentMana;
    [SerializeField] float spentMana = 10f;
    
    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHP;

    static public float HP;
    static public float MANA;
    static public bool hasMana;
    static public bool isAlive;


    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
        currentHP = maxHP;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        MANA = currentMana;
        HP = currentHP;

        // HP
        if (currentHP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
            

        // Mana regen
        if (currentMana < maxMana)
            currentMana += Time.deltaTime * 0.5f;

        // If the character fires, loses spentMana
        if (CharacterFire.fire)
            currentMana -= spentMana;

        // can't fire if the character doesn't have enough mana
        if (currentMana - spentMana > 0)
            hasMana = true;
        else
            hasMana = false;
    }
}
