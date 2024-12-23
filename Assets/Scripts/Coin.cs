using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            gameManager.pointUP();
        }
    }
}
