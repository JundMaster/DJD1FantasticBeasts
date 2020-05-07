using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] Text currentHP;
    [SerializeField] Text currentMana;

    int HP;
    int mana;
    // Start is called before the first frame update
    void Start()
    {
        
        mana = (int)(CharacterInfo.MANA);
        HP = (int)(CharacterInfo.HP);
        currentHP.text = "HP: " + HP.ToString();
        currentMana.text = "MANA: " + mana.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        HP = (int)(CharacterInfo.HP);
        mana = (int)(CharacterInfo.MANA);

        if (CharacterInfo.HP > 0)
            currentHP.text = "HP: " + HP.ToString();

        if (CharacterInfo.MANA > 0)
            currentMana.text = "MANA: " + mana.ToString();

    }
}
