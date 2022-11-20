using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text timeText;
    private float timeElapsed;

    void Awake()
    {
        timeElapsed = 0f;
        timeText = GetComponent<TMP_Text>();
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
        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
