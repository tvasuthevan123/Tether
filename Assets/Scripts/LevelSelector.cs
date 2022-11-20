using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public void Select(string LevelName)
    {
        Debug.Log("Moving to " + LevelName);
        fader.FadeTo(LevelName);
    }
}
