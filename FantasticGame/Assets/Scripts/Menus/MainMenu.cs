using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

sealed public class MainMenu : MonoBehaviour
{
    // To get the last selected button
    private GameObject selectedButton;

    private void Start()
    {
        // Last selected button
        selectedButton = new GameObject();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset statics
        ResetStatics();
    }

    private void Update()
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

    public void PlayGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetStatics()
    {
        LevelManager.NewtLives = 3;
        LevelManager.CreaturesSaved = 0;
        LevelManager.GAMEOVER = false;
        LevelManager.AssistMode = false;
        LevelManager.WONGAME = false;
        LevelManager.ReachedBoss = false;
        LevelManager.BossDefeated = false;
        LevelManager.CUTSCENE = false;
        PauseMenu.gamePaused = false;
    }

    public void PlaySoundButtonSelect()
    {
        SoundManager.PlaySound(AudioClips.buttonSelected);
    }
    public void PlaySoundButtonScroll()
    {
        SoundManager.PlaySound(AudioClips.buttonScroll);
    }
}
