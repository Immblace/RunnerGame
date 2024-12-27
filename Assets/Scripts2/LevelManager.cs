using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private TextMeshProUGUI recordText;
    private ScoreManager scoreManager;

    void Start()
    {
        Time.timeScale = 1;
        if (loseMenu != null)
        {
            loseMenu.SetActive(false);
        }
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    

    public void onLoseMenu()
    {
        Time.timeScale = 0;
        recordText.text = "Record: " + scoreManager.getCurrentRecord();
        loseMenu.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
