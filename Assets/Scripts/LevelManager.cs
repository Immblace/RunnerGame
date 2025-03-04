using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject LoadingUI;
    [SerializeField] private GameObject TapText;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI loadingText;


    public void RestartScene()
    {
        StartCoroutine(AsyncLoadingScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void MainMenu()
    {
        StartCoroutine(AsyncLoadingScene(0));
    }


    public void StartGame()
    {
        StartCoroutine(AsyncLoadingScene(1));
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    private IEnumerator AsyncLoadingScene(int SceneIndex)
    {
        LoadingUI.SetActive(true);
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(SceneIndex);
        asyncScene.allowSceneActivation = false;

        while (asyncScene.progress < 0.9f)
        {
            loadingText.text = $"Loading: {Mathf.Clamp01(asyncScene.progress / 0.9f) * 100}";
            slider.value = Mathf.Clamp01(asyncScene.progress / 0.9f);
            yield return null;
        }

        slider.value = 1;
        loadingText.text = "Loading: 100%";

        yield return new WaitForSecondsRealtime(1.5f);
        TapText.SetActive(true);

        while (!Input.anyKey)
        {
            yield return null;
        }

        asyncScene.allowSceneActivation = true;
    }


}
