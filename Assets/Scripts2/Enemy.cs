using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private Transform[] enemyTarget = new Transform[3];
    [SerializeField] private Transform[] BlockPos = new Transform[2];
    [SerializeField] private Transform jumpBlock;
    [SerializeField] private Transform bulletPrefab;

    private GameObject enemy;
    private Transform lookPos;
    private bool startFire;
    private Color[] color = { Color.red, Color.black, Color.yellow, Color.white, Color.blue, Color.green, Color.grey };


    private void Start()
    {
        if (Random.Range(0,100) > 70)
        {
            enemy = Instantiate(enemyPrefab, enemyPosition.position, Quaternion.identity);
            lookPos = enemyTarget[Random.Range(0, enemyTarget.Length)];
            enemy.transform.LookAt(lookPos);
        }

        if (Random.Range(0,100) > 80)
        {
            Instantiate(jumpBlock, BlockPos[Random.Range(0, BlockPos.Length)].position, transform.rotation);
        }
    }



    private void Update()
    {
        if (startFire)
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        Transform newBullet = Instantiate(bulletPrefab, enemy.transform.position, enemy.transform.rotation);
    }

    private IEnumerator switchColor()
    {
        for (int i = 0; i < Random.Range(8,16); i++)
        {
            enemy.GetComponent<Renderer>().material.color = color[Random.Range(0, color.Length)];
            yield return new WaitForSeconds(0.1f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && enemy != null)
        {
            StartCoroutine(switchColor());
            startFire = true;
        }
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            startFire = false;
            Destroy(enemy);
        }
    }
}
