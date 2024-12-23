using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _pointText;
    [SerializeField] private TextMeshProUGUI _record_Text;
    [SerializeField] private GameObject DeadMenu;
    private int point;
    private float timer = 3f;
    public static bool isStarted;
    public bool endGame;
    public static float speed = 10f;
    private float timetoUp = 5f;


    private void Start()
    {
        Time.timeScale = 1f;
        DeadMenu.SetActive(false);
        isStarted = false;
        _timeText.text = timer.ToString();
        _pointText.text = "Point: " + point;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                _timeText.text = "";
                isStarted = true;
            }
            else
            {
                _timeText.text = Mathf.Round(timer).ToString();
            }
        }

        if (endGame)
        {
            exitMenu();
            endGame = false;
        }



        if (timetoUp > 0)
        {
            timetoUp -= Time.deltaTime;
            
            if (timetoUp <= 0)
            {
                speed += 1.5f;
                timetoUp = 3f + Random.Range(0, 2.5f);
            }
        }
    }

    private void exitMenu()
    {
        _record_Text.text = "Record: " + point;
        Time.timeScale = 0f;
        DeadMenu.SetActive(true);
    }


    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void pointUP()
    {
        point++;
        _pointText.text = "Point: " + point;
    }
}
