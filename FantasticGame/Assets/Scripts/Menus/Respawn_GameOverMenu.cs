using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

sealed public class Respawn_GameOverMenu : MonoBehaviour
{
    // Button selected
    // CREATES AN EMPTY GAMEOBJECT IN THE HIERARCHY //
    private GameObject selectedButton;

    // Both menus
    [SerializeField] public GameObject checkpointRespawnMenu;
    [SerializeField] private GameObject gameOverMenu;

    // Tells if the player is currently in the respawn menu
    public static bool InRespawnMenu { get; private set; } = false;

    // Player
    private Player p1;

    void Start()
    {
        p1 = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (p1 == null)
        {
            p1 = FindObjectOfType<Player>();
        }


        if (checkpointRespawnMenu.activeSelf || gameOverMenu.activeSelf)
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

        
        // Respawn ( only if the game isn't over ) or gameover
        if (p1.Stats.IsAlive == false)
        {
            if (LevelManager.GAMEOVER)
            {
                InRespawnMenu = true;
                GameOver();
            }
            else
            {
                InRespawnMenu = true;
                RespawnMenu();
            }
        }
    }


    public void LoadCheckpoint()
    {
        // Takes a life, disables the menu, sets the timescale to normal
        LevelManager.NewtLives--;
        checkpointRespawnMenu.SetActive(false);
        Time.timeScale = 1f;
        InRespawnMenu = false;
    }

    public void Restart()
    {
        // Loads the same level
        Time.timeScale = 1f;
        PauseMenu.gamePaused = false;
        InRespawnMenu = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        // Goes to main menu
        Application.Quit();
    }

    private void RespawnMenu()
    {
        Time.timeScale = 0f;
        checkpointRespawnMenu.SetActive(true);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }
}
