using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnGenerate : MonoBehaviour
{
    [SerializeField] private List<Transform> Roads = new List<Transform>(10);
    [SerializeField] private List<Transform> Turns = new List<Transform>(10);
    [SerializeField] private Transform[] Rotate = new Transform[2];
    [SerializeField] private Transform TrackPrefab;
    [SerializeField] private float distance = 1.67f;
    [SerializeField] private float dist;

    private bool Generate = true;
    private int countRight = 0;
    private int countLeft = 0;


    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            TrackGenerate();
        }
    }

    private void Update()
    {
        if (Turns.Count < 15 && Generate)
        {
            TrackGenerate();
        }
    }

    private void TrackGenerate()
    {
        Transform newRotate = Instantiate(Rotate[Random.Range(0, Rotate.Length)]);
        newRotate.position = Roads.Last().TransformPoint(Vector3.forward / distance);
        newRotate.rotation = Roads.Last().rotation;
        Turns.Add(newRotate);

        if (Turns.Last().gameObject.tag == "Right")
        {
            countLeft = 0;
            countRight++;

            Debug.Log("right " + countRight);

            if (countRight > 2)
            {
                Debug.Log("destroyRight");
                Generate = false;
                Destroy(Turns.Last().gameObject);
                Turns.Remove(Turns.Last().transform);
                generateLeft();
                Generate = true;
                countRight = 0;
                return;
            }
        }
        else
        {
            countRight = 0;
            countLeft++;

            Debug.Log("left " + countLeft);

            if (countLeft > 2)
            {
                Debug.Log("destroyLeft");
                Generate = false;
                Destroy(Turns.Last().gameObject);
                Turns.Remove(Turns.Last().transform);
                generateRight();
                Generate = true;
                countLeft = 0;
                return;
            }
        }

        if (Turns.Last().gameObject.tag == "Right")
        {
            Transform newTrack = Instantiate(TrackPrefab);
            newTrack.position = Turns.Last().TransformPoint(Vector3.right / dist);
            newTrack.rotation = Roads.Last().rotation;
            newTrack.Rotate(Vector3.up , 90);
            Roads.Add(newTrack);
        }
        else
        {
            Transform newTrack = Instantiate(TrackPrefab);
            newTrack.position = Turns.Last().TransformPoint(Vector3.left / dist);
            newTrack.rotation = Roads.Last().rotation;
            newTrack.Rotate(Vector3.up , -90);
            Roads.Add(newTrack);
        }
    }

    private void generateRight()
    {
        Transform newRotate = Instantiate(Rotate[1]);
        newRotate.position = Roads.Last().TransformPoint(Vector3.forward / distance);
        newRotate.rotation = Roads.Last().rotation;
        Turns.Add(newRotate);

        Transform newTrack = Instantiate(TrackPrefab);
        newTrack.position = Turns.Last().TransformPoint(Vector3.right / dist);
        newTrack.rotation = Roads.Last().rotation;
        newTrack.Rotate(Vector3.up, 90);
        Roads.Add(newTrack);
    }

    private void generateLeft()
    {
        Transform newRotate = Instantiate(Rotate[0]);
        newRotate.position = Roads.Last().TransformPoint(Vector3.forward / distance);
        newRotate.rotation = Roads.Last().rotation;
        Turns.Add(newRotate);

        Transform newTrack = Instantiate(TrackPrefab);
        newTrack.position = Turns.Last().TransformPoint(Vector3.left / dist);
        newTrack.rotation = Roads.Last().rotation;
        newTrack.Rotate(Vector3.up, -90);
        Roads.Add(newTrack);
    }

    public void ResetAll()
    {
        Roads.Clear();
        Turns.Clear();

    }

    public void DeletObj()
    {
        Destroy(Roads.First().transform.gameObject, 1f);
        Destroy(Turns.First().transform.gameObject, 1f);
        Roads.Remove(Roads.First().transform);
        Turns.Remove(Turns.First().transform);
    }

}
