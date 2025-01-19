using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private Transform[] enemyTarget = new Transform[3];
    [SerializeField] private Transform[] BlockPos = new Transform[2];
    [SerializeField] private Transform jumpBlock;

    private SpawnGenerate spawnGenerate;
    private Enemy enemy;
    private Transform lookPos;


    private void Start()
    {
        spawnGenerate = FindObjectOfType<SpawnGenerate>();

        if (spawnGenerate.CheckTimeToStartSpawn() > 2)
        {
            if (Random.Range(0, 100) > 70)
            {
                enemy = Instantiate(enemyPrefab, enemyPosition.position, Quaternion.identity);
                lookPos = enemyTarget[Random.Range(0, enemyTarget.Length)];
                enemy.transform.LookAt(lookPos);
            }
        }

        if (spawnGenerate.CheckTimeToStartSpawn() > 4)
        {
            if (Random.Range(0, 100) > 80)
            {
                Instantiate(jumpBlock, BlockPos[Random.Range(0, BlockPos.Length)].position, transform.rotation);
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
    }
}
