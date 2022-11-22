using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public Button[] levelButtons;

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        for(int i = 0; i < levelButtons.Length; i++)
        {
            if(i+1 > levelReached)
                levelButtons[i].interactable = false;
        }
    }

    public void Select(string LevelName)
    {
        Debug.Log("Moving to " + LevelName);
        fader.FadeTo(LevelName);
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().IGMStopMusic();
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().LSPlayMusic();
    }

    public void Home()
    {
        fader.FadeTo("Home");
    }
}
