using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] RectTransform healthBar;
    [SerializeField] RectTransform manaBar;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        player = FindObjectOfType<Player>();

        if (manaBar)
        {
            manaBar.localScale = new Vector3(player.CurrentMana / 100f, 1f, 1f);
        }
        if (healthBar)
        {
            healthBar.localScale = new Vector3(player.CurrentHP / 100f, 1f, 1f);
        }
    }
}




















    /*
    [SerializeField] TextMeshProUGUI currentHP;
    [SerializeField] TextMeshProUGUI currentMana;

    int HP;
    int mana;
    Stats stats;






    */


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

