using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject playerUI;
    public TMP_Text helpText;

    public SceneFader sceneFader;

    public AudioMixer audioMixer;
    public GrapplingGun grapplingGun;

    void Start()
    {
        if(grapplingGun.isTooltipEnabled)
        {
            helpText.text = "HELP TEXT : ENABLED";
            helpText.color = Color.red;
        }
        else
        {
            helpText.text = "HELP TEXT : DISABLED";
            helpText.color = Color.green;
        }
    }
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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
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

    void toggleCrosshairHelp()
    {
        if(grapplingGun.isTooltipEnabled)
        {
            helpText.text = "HELP TEXT : DISABLED";
            helpText.color = Color.red;
        }
        else
        {
            helpText.text = "HELP TEXT : ENABLED";
            helpText.color = Color.green;
        }
    }
}
