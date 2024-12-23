using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject PlatPrefab;
    [SerializeField] private List<Transform> Platforms = new List<Transform>();
    [SerializeField] private Transform PlatBags;
    private int maxCount = 20;

    private void Start()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GeneratePlatform();
        }
    }

    private void Update()
    {
        if (Platforms.Count < maxCount)
        {
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        Transform newPlat = Instantiate(PlatPrefab.transform, Platforms.Last().position + Vector3.forward * PlatPrefab.transform.localScale.z, Quaternion.identity);
        Platforms.Add(newPlat);
        newPlat.transform.SetParent(PlatBags);
    }


    private void OnTriggerEnter(Collider other)
    {
        Platforms.Remove(other.transform);
        Destroy(other.gameObject);
    }
}
