using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform bulletPrefab;

    private Renderer enemyColor;
    private Color[] color = { Color.red, Color.black, Color.yellow, Color.white, Color.blue, Color.green, Color.grey };
    private bool startFire;


    private void Start()
    {
        enemyColor = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (startFire)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }

    public void Shoot()
    {
        StartCoroutine(switchColor());
        startFire = true;
    }

    private IEnumerator switchColor()
    {
        for (int i = 0; i < Random.Range(12 , 16); i++)
        {
            enemyColor.material.color = color[Random.Range(0, color.Length)];
            yield return new WaitForSeconds(0.1f);
        }
    }

}
