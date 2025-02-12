using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform[] coinPosition = new Transform[15];
    [SerializeField] private List<Transform> coinBags = new List<Transform>(3);
    [SerializeField] private Transform[] enemyTarget = new Transform[3];
    [SerializeField] private Transform[] BlockPos = new Transform[2];
    [SerializeField] private Transform blockPrefab;

    private Enemy enemy;
    private GameObject jumpBlock;
    private Transform lookPos;
    private bool coinActive;


    private void Start()
    {
        if (Random.Range (0,100) > 30)
        {
            if (Random.Range(0, 100) > 50)
            {
                enemy = Instantiate(enemyPrefab, enemyPosition.position, Quaternion.identity);
                lookPos = enemyTarget[Random.Range(0, enemyTarget.Length)];
                enemy.transform.LookAt(lookPos);
            }

            if (Random.Range(0, 100) > 45)
            {
                jumpBlock = Instantiate(blockPrefab.gameObject, BlockPos[Random.Range(0, BlockPos.Length)].position, transform.rotation);
            }

            if (Random.Range(0,100) > 40)
            {
                int newCoinPosition = Random.Range(1, 4);
                coinActive = true;

                switch (newCoinPosition)
                {
                    case 1:
                        for (int i = 0; i < 5; i++)
                        {
                            Transform newCoin = Instantiate(coinPrefab, coinPosition[i].position, Quaternion.identity);
                            coinBags.Add(newCoin);
                        }
                        break;
                    case 2:
                        for (int i = 5; i < 10; i++)
                        {
                            Transform newCoin = Instantiate(coinPrefab, coinPosition[i].position, Quaternion.identity);
                            coinBags.Add(newCoin);
                        }
                        break;
                    case 3:
                        for (int i = 10; i < 15; i++)
                        {
                            Transform newCoin = Instantiate(coinPrefab, coinPosition[i].position, Quaternion.identity);
                            coinBags.Add(newCoin);
                        }
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && enemy != null)
        {
            enemy.Shoot();
        }
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }

        if (jumpBlock != null)
        {
            Destroy(jumpBlock.gameObject);
        }

        if (coinActive)
        {
            foreach (var item in coinBags)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}
