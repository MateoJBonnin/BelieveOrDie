using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Image fadeImage;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(int index)
    {
        StartCoroutine(FadeOutIn(index));
    }

    private IEnumerator FadeOutIn(int index)
    {
        fadeImage.color = Color.clear;
        fadeImage.gameObject.SetActive(true);

        for (float t = 0; t < 1; t+=Time.deltaTime)
        {
            fadeImage.color = Color.Lerp(Color.clear, Color.black, t);
            yield return new WaitForEndOfFrame();
        }

        yield return SceneManager.LoadSceneAsync(index);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            fadeImage.color = Color.Lerp(Color.black, Color.clear, t);
            yield return new WaitForEndOfFrame();
        }

        fadeImage.color = Color.clear;
        fadeImage.gameObject.SetActive(false);
    }
}

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}