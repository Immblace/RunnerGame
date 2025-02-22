using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static int SceneIndex;
    


    public void RestartScene()
    {
        SceneIndex = 2;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneIndex = 0;
        SceneManager.LoadScene(1);
    }


    public void StartGame()
    {
        SceneIndex = 2;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
