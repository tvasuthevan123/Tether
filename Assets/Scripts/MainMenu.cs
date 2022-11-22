using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneFader sceneFader;

    public void LoadLevelSelect()
    {
        PauseMenu.isGamePaused = false;
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
