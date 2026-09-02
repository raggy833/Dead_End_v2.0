using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene instance;
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    public float loadingTime = 2f; // Set the minimum loading time in seconds

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        LoadingScreen.SetActive(true);

        float startTime = Time.time;

        while (!operation.isDone || Time.time - startTime < loadingTime)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
    public void ShowLoadScreen()
    {
        if (LoadingScreen != null)
        {
            LoadingScreen.SetActive(true);
        }
        else
        {
            Debug.Log("Loading screen game object is null");
        }


    }
}
