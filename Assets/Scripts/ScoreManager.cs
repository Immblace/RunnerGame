using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myScore;
    [SerializeField] private TextMeshProUGUI myRecord;
    [SerializeField] private TextMeshProUGUI myPoints;

    [SerializeField] private GameObject loseMenu;
    [SerializeField] private TextMeshProUGUI recordText;

    private float score = 0;
    private float points = 0;


    void Start()
    {
        Time.timeScale = 1;

        if (myRecord != null)
        {
            myRecord.text = "Record: " + Math.Round(PlayerPrefs.GetFloat("MyRecord"));
        }

        if (loseMenu != null)
        {
            loseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (myScore != null)
        {
            score += Time.deltaTime;
            myScore.text = "Score: " + Math.Round(score);
        }
    }

    public void onLoseMenu()
    {
        Time.timeScale = 0;
        recordText.text = "Record: " + Math.Round(PlayerPrefs.GetFloat("MyRecord"));
        loseMenu.SetActive(true);
    }

    public void UpPoints()
    {
        points++;
        myPoints.text = "Points: " + points.ToString();
    }
    
    public void CheckRecord()
    {
        if (score > PlayerPrefs.GetFloat("MyRecord"))
        {
            PlayerPrefs.SetFloat("MyRecord", score);
        }
    }



}
