using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour
{
    [SerializeField] private GameObject turnsWall;





    public void DestroyTurnWall()
    {
        Destroy(turnsWall.gameObject);
    }
}
