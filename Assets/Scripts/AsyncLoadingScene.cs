using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI PressAnyKeyText;

    private AsyncOperation asyncScene;
    

    private void Start()
    {
        StartCoroutine(startAsyncScene(LevelManager.SceneIndex));
    }

    

    private IEnumerator startAsyncScene(int sceneNumber)
    {
        asyncScene = SceneManager.LoadSceneAsync(sceneNumber);
        asyncScene.allowSceneActivation = false;

        while (asyncScene.progress < .9f)
        {
            loadingText.text = "Loading: " + Mathf.Clamp01(asyncScene.progress / .9f * 100);
            slider.value = Mathf.Clamp01(asyncScene.progress / .9f);
            yield return null;
        }

        slider.value = 1;
        loadingText.text = "Loading: 100%";
        yield return new WaitForSecondsRealtime(1.5f);
        PressAnyKeyText.gameObject.SetActive(true);

        while (!Input.anyKey)
        {
            yield return null;
        }

        asyncScene.allowSceneActivation = true;
    }

}
