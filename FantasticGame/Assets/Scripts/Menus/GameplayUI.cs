using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

sealed public class GameplayUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform manaBar;

    [SerializeField] private TextMeshProUGUI nifflerScore;
    [SerializeField] private TextMeshProUGUI newtLives;

    // Boss
    [SerializeField] private GameObject bossHealthBarBase;
    [SerializeField] private RectTransform bossHealthBar;
    private bool bossEncounterFirstTime  = true;
    private float hpBossBar = 0;
    //////////////////////////////////

    private Player player;
    private Boss boss;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        bossHealthBarBase.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        // BARS UI
        if (player != null)
        {
            if (manaBar)
            {
                manaBar.localScale = new Vector3(player.CurrentMana / 100f, 1f, 1f);
            }
            if (healthBar)
            {
                healthBar.localScale = new Vector3(player.CurrentHP / 100f, 1f, 1f);
            }
        }

        // NiFFLER UI
        nifflerScore.text = $"{LevelManager.CreaturesSaved} / 10";

        // NEWT UI
        if (LevelManager.AssistMode == false) newtLives.text = "x" + LevelManager.NewtLives;
        else newtLives.text = "xx";

        // BOSS UI
        if (LevelManager.reachedBoss)
        {
            // Finds Boss
            if (boss == null)
            {
                boss = FindObjectOfType<Boss>();
            }

            // Sets hp bar as active
            bossHealthBarBase.SetActive(true);
            if (bossEncounterFirstTime)
            {
                if (bossHealthBar != null) bossHealthBar.localScale = new Vector3(0f, 1f, 1f);
                // Increments the bar size as time passes, then turns first time to false
                if (hpBossBar > 0.95f)
                {
                    bossEncounterFirstTime = false;
                }
                else if (hpBossBar < 1)
                {
                    hpBossBar += 0.8f * Time.deltaTime;
                    if (bossHealthBar != null) bossHealthBar.localScale += new Vector3(hpBossBar, 0f, 0f);
                }
            }

            else
            {   // Sets the bar with boss hp
                if (boss != null) bossHealthBar.localScale = new Vector3(boss.Stats.CurrentHP / 1000f, 1f, 1f);
            }
        }

        if (Boss.BossDefeated || LevelManager.reachedBoss == false) // If the fight ends, turns off the bar
        {
            hpBossBar = 0;
            bossEncounterFirstTime = true;
            bossHealthBarBase.SetActive(false);
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

