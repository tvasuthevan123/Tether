using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve fadeCurve;

    void Start() 
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    
    IEnumerator FadeIn()
    {
        float time = 1f;
        while(time > 0)
        {
            time-=Time.deltaTime;
            float a = fadeCurve.Evaluate(time);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float time = 0f;
        while(time < 1)
        {
            time+=Time.deltaTime;
            float a = fadeCurve.Evaluate(time);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
}