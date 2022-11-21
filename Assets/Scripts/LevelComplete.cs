using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public Material redGemMat;
    public static float timeTaken;
    public TMP_Text timeDisplay;
    public TMP_Text[] gemTimes;
    public static string levelName;
    public static string nextLevel;
    public SceneFader sceneFader;

    public GameObject[] gems;

    private static Dictionary<string, float[]> levelTimes = new Dictionary<string, float[]>{
        ["Tutorial1"] = new[] {100f,100f,100f},
        ["Tutorial2"] = new[] {11f,14.25f,17.5f},
        ["Tutorial3"] = new[] {8f,14f,20f},
        ["Tutorial4"] = new[] {4f,7.5f,11f},
        ["Tutorial5"] = new[] {9f,15.75f,21f}
    };

    void Start()
    {
        for(int i=0; i<=2; i++)
        {
            gemTimes[i].text = getTimeFromFloat(levelTimes[levelName][i]);
        }
        StartCoroutine(FillGems(timeTaken));
        StartCoroutine(DisplayTime(timeTaken));
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject gem in gems)
        {
            gem.transform.Rotate(new Vector3(0,0,70) * Time.deltaTime);
        }
    }

    IEnumerator FillGems(float timeTaken)
    {
        float[] times = levelTimes[levelName];
        Material[] mats = {redGemMat};
        for(int i=0; i<=2; i++)
        {
            if(timeTaken<times[i])
            {
                gems[i].GetComponent<MeshRenderer>().materials = mats;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator DisplayTime(float timeTaken)
    {
        float time = 0f;
        while(time<2f)
        {
            time+=Time.deltaTime;
            float timeToDisplay = Mathf.Lerp(0, timeTaken, time/2);
            timeDisplay.text = getTimeFromFloat(timeToDisplay);
            yield return 0;
        }
    }

    string getTimeFromFloat(float time)
    {
        float minutes = Mathf.FloorToInt(time/60);
        float seconds = Mathf.FloorToInt(time%60);
        float milliseconds = time % 1 * 1000;
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
