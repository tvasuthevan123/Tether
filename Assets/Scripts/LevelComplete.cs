using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public Material redGemMat;
    public static float timeTaken;
    public TMP_Text timeDisplay, oneGemTime, twoGemTime, threeGemTime;
    public static string levelName;
    public static string nextLevel;
    public SceneFader sceneFader;

    public GameObject gem1, gem2, gem3;

    private static Dictionary<string, float[]> levelTimes = new Dictionary<string, float[]>{
        ["Tutorial1"] = new[] {100f,100f,100f}
    };

    void Start()
    {
        oneGemTime.text = getTimeFromFloat(levelTimes[levelName][0]);
        twoGemTime.text = getTimeFromFloat(levelTimes[levelName][1]);
        threeGemTime.text = getTimeFromFloat(levelTimes[levelName][2]);
        StartCoroutine(FillGems(timeTaken));
        StartCoroutine(DisplayTime(timeTaken));
    }

    // Update is called once per frame
    void Update()
    {
        gem1.transform.Rotate(new Vector3(0,0,70) * Time.deltaTime);
        gem2.transform.Rotate(new Vector3(0,0,70) * Time.deltaTime);
        gem3.transform.Rotate(new Vector3(0,0,70) * Time.deltaTime);
    }

    IEnumerator FillGems(float timeTaken)
    {
        float[] times = levelTimes[levelName];
        Material[] mats = {redGemMat};
        if(timeTaken<times[2])
        {
            gem1.GetComponent<MeshRenderer>().materials = mats;
            yield return new WaitForSeconds(0.5f);
            gem2.GetComponent<MeshRenderer>().materials = mats;
            yield return new WaitForSeconds(0.5f);
            gem3.GetComponent<MeshRenderer>().materials = mats;
        }
        else if(timeTaken<times[1])
        {
            gem1.GetComponent<MeshRenderer>().materials = mats;
            yield return new WaitForSeconds(0.5f);
            gem2.GetComponent<MeshRenderer>().materials = mats;
        }
        if(timeTaken<times[0])
        {
            gem1.GetComponent<MeshRenderer>().materials = mats;
            yield return new WaitForSeconds(0.5f);
            gem2.GetComponent<MeshRenderer>().materials = mats;
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
