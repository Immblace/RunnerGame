using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{


    private SpawnGenerate spawnDelete;


    private void Start()
    {
        spawnDelete = FindObjectOfType<SpawnGenerate>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            spawnDelete.DeletObj();
        }
    }


}
