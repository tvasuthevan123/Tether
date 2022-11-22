using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    public AudioSource IGM;
    public AudioSource LevelSelect;

    private void Awake()
    {
    
    }
    
    public void IGMPlayMusic()
    {
        if(LevelSelect.isPlaying || IGM.isPlaying) return;
        IGM.Play();
    }

    public void IGMStopMusic()
    {
        IGM.Stop();
    }
    public void LSPlayMusic()
    {
        if(LevelSelect.isPlaying || IGM.isPlaying) return;
        LevelSelect.Play();
    }

    public void LSStopMusic()
    {
        LevelSelect.Stop();
    }
}
