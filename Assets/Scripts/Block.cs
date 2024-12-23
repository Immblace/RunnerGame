using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    private GameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            manager.endGame = true;
        }
    }
}
