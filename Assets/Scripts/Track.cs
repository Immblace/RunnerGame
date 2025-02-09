using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform[] coinPosition = new Transform[9];
    [SerializeField] private List<Transform> coinBags = new List<Transform>(3);
    [SerializeField] private Transform[] enemyTarget = new Transform[3];
    [SerializeField] private Transform[] BlockPos = new Transform[2];
    [SerializeField] private Transform blockPrefab;

    private SpawnGenerate spawnGenerate;
    private Enemy enemy;
    private GameObject jumpBlock;
    private Transform lookPos;
    private bool coinActive;


    private void Start()
    {
        spawnGenerate = FindObjectOfType<SpawnGenerate>();

        if (spawnGenerate.getCurrentSpawnTime() >= 3)
        {
            if (Random.Range(0, 100) > 55)
            {
                enemy = Instantiate(enemyPrefab, enemyPosition.position, Quaternion.identity);
                lookPos = enemyTarget[Random.Range(0, enemyTarget.Length)];
                enemy.transform.LookAt(lookPos);
            }

            if (Random.Range(0, 100) > 65)
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
                        for (int i = 0; i < 3; i++)
                        {
                            Transform newCoin = Instantiate(coinPrefab, coinPosition[i].position, Quaternion.identity);
                            coinBags.Add(newCoin);
                        }
                        break;
                    case 2:
                        for (int i = 3; i < 6; i++)
                        {
                            Transform newCoin = Instantiate(coinPrefab, coinPosition[i].position, Quaternion.identity);
                            coinBags.Add(newCoin);
                        }
                        break;
                    case 3:
                        for (int i = 6; i < 9; i++)
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
