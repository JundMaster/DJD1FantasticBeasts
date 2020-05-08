using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentHP;
    [SerializeField] TextMeshProUGUI currentMana;

    int HP;
    int mana;
    Stats stats;
    /*
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();

        mana = (int)(stats.MANA);
        HP = (int)(stats.HP);
        currentHP.text = "HP: " + HP.ToString();
        currentMana.text = "MANA: " + mana.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        HP = (int)(stats.HP);
        mana = (int)(stats.MANA);

        if (stats.HP > 0)
            currentHP.text = "HP: " + HP.ToString();

        if (stats.MANA > 0)
            currentMana.text = "MANA: " + mana.ToString();

    }
    */
}
