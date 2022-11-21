using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject playerUI;

    public SceneFader sceneFader;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("Paused");
    }
    
    public void Resume()
    {
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Resuming?");
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        Time.timeScale=1f;
    }

    public void LoadLevelSelect()
    {
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale=1f;
        sceneFader.FadeTo("LevelSelect");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
