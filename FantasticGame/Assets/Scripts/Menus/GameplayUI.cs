using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    private Boss_02 boss_02;

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
        else if (LevelManager.GAMEOVER) newtLives.text = "x0";
        else newtLives.text = "xx";

        // BOSS UI
        if (LevelManager.ReachedBoss)
        {
            // Finds Bosses
            if (boss == null)
            {
                boss = FindObjectOfType<Boss>();
            }
            if (boss_02 == null)
            {
                boss_02 = FindObjectOfType<Boss_02>();
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

                // Level 01
                if (SceneManager.GetActiveScene().name == "Level01")
                {
                    if (boss != null)
                    {
                        bossHealthBar.localScale = new Vector3(boss.Stats.CurrentHP / boss.Stats.MaxHP, 1f, 1f);
                    }
                }

                // Level 02
                else if (SceneManager.GetActiveScene().name == "Level02")
                {
                    if (boss_02 != null)
                    {
                        bossHealthBar.localScale = new Vector3(boss_02.Stats.CurrentHP / boss_02.Stats.MaxHP, 1f, 1f);
                    }
                }
            }
        }

        if (LevelManager.BossDefeated || LevelManager.ReachedBoss == false) // If the fight ends, turns off the bar
        {
            hpBossBar = 0;
            bossEncounterFirstTime = true;
            bossHealthBarBase.SetActive(false);
        }
    }
}