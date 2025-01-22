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

    private float score = 0;


    void Start()
    {
        myRecord.text = "Record: " + Math.Round(PlayerPrefs.GetFloat("MyRecord"));
    }



    void Update()
    {
        if (myScore != null)
        {
            score += Time.deltaTime;
            myScore.text = "Score: " + Math.Round(score);
        }
    }



    public void CheckRecord()
    {
        if (score > PlayerPrefs.GetFloat("MyRecord"))
        {
            PlayerPrefs.SetFloat("MyRecord", score);
        }
    }


}
