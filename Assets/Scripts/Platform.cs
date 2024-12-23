using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    
    [SerializeField] private List<Transform> positions = new List<Transform>();
    [SerializeField] private GameObject coinPrefabs;
    [SerializeField] private GameObject blockPrefabs;
    private bool Generate = true;
    private int spawnPos;


    private void Start()
    {
        spawnPos = Random.Range(0, positions.Count);

        if (1 > Random.Range(0, 3))
        {
            Transform newCoin = Instantiate(coinPrefabs.transform, positions[spawnPos].position, Quaternion.identity);
            newCoin.SetParent(transform);
        }
        
        if (1 > Random.Range(0, 4))
        {
            while (Generate)
            {
                int newPos = Random.Range(0, positions.Count);
                if (newPos != spawnPos)
                {
                    Transform newBlock = Instantiate(blockPrefabs.transform, positions[newPos].position, Quaternion.identity);
                    newBlock.transform.SetParent(transform);
                    Generate = false;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.isStarted)
        {
            transform.Translate(Vector3.back * GameManager.speed * Time.fixedDeltaTime);
        }

    }

}
