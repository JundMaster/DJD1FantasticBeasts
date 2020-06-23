using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

sealed public class MainMenu : MonoBehaviour
{
    private GameObject selectedButton;

    private void Start()
    {
        selectedButton = new GameObject();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    public static void PlayGame()
    {
        SceneManager.LoadScene("Final");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
