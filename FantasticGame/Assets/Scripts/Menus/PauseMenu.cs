using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

sealed public class PauseMenu : MonoBehaviour
{
    // Button selected
    // CREATES AN EMPTY GAMEOBJECT IN THE HIERARCHY //
    private GameObject selectedButton;

    // Variable to know if the game is paused
    static public bool gamePaused;

    // Menus //
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject AssistModeMenu;
    [SerializeField] private GameObject OptionsMenu;

    // Text on assist mode menu
    [SerializeField] private TextMeshProUGUI infHP;
    [SerializeField] private TextMeshProUGUI infMana;
    [SerializeField] private TextMeshProUGUI infLives;

    // Player reference
    Player p1;
    private void Awake()
    {
        selectedButton = new GameObject();
        p1 = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gamePaused && LevelManager.WONGAME == false)
        {
            // Keeps a button selected
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(selectedButton);
            }
            else
            {
                selectedButton = EventSystem.current.currentSelectedGameObject;
            }
        }

        if (p1 == null) p1 = FindObjectOfType<Player>();

        // Only if the player is alive and the game isn't over
        if (p1 != null && p1.Stats.IsAlive && Respawn_GameOverMenu.InRespawnMenu == false)
        {
            // When the player presses ESC
            if (Input.GetKeyDown(KeyCode.Escape) && LevelManager.WONGAME == false)
            {
                if (gamePaused)
                {   // If the player isn't in main pause menu
                    if (!AssistModeMenu.activeSelf && !OptionsMenu.activeSelf)
                    {
                        Resume();
                    }
                }
                else
                {
                    Pause();
                }
            }
        }


        // Updates assist mode text
        if (p1 != null)
        {
            if (p1.GodMode) infHP.text = "infinite hp: on";
            else infHP.text = "infinite hp: off";
            if (p1.InfiniteMana) infMana.text = "infinite mana: on";
            else infMana.text = "infinite mana: off";
        }
        if (LevelManager.AssistMode) infLives.text = "infinite lives: on";
        else infLives.text = "infinite lives: off";
    }

    public void InfiniteHP()
    {
        p1.GodMode = !p1.GodMode;
    }

    public void InfiniteMana()
    {
        p1.InfiniteMana = !p1.InfiniteMana;
    }

    public void InfiniteLives()
    {
        LevelManager.AssistMode = !LevelManager.AssistMode;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
