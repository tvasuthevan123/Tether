using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timeValue;
    public TMP_Text timeLabel;
    public float timeElapsed;

    void Awake()
    {
        timeElapsed = 0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        DisplayTime(timeElapsed);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay/60);
        float seconds = Mathf.FloorToInt(timeToDisplay%60);
        float milliseconds = timeToDisplay % 1 * 1000;
        timeValue.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void ReduceTime(float timeToReduce)
    {
        if(timeToReduce < timeElapsed)
            timeElapsed-=timeToReduce;
        else
            timeElapsed=0f;
        timeLabel.color = Color.green;
        timeValue.color = Color.green;
        StartCoroutine(PickUpFade());
    }

    IEnumerator PickUpFade()
    {
        float time = 0f;
        while(time < 1)
        {
            time+=Time.deltaTime;
            
            timeLabel.color = Color.Lerp(Color.green, Color.white, time);
            timeValue.color = Color.Lerp(Color.green, Color.white, time);
            yield return 0;
        }
    }
}
