using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour
{
    [SerializeField] private GameObject turnsWall;
    [SerializeField] private Transform PlayerPos;





    public void DestroyTurnWall()
    {
        Destroy(turnsWall.gameObject);
    }

    public Transform GetPlayerPos()
    {
        return PlayerPos;
    }
}
