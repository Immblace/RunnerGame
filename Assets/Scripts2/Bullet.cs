using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float speed = 12f;

    private void Start()
    {
        Destroy(gameObject, 1.8f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    
}
