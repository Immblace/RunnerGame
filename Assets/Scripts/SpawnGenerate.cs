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
    private float distanceToTrack = 1.84f;
    private float distanceToTurn = 0.1667f;

    private bool Generate = true;
    private int countRight = 0;
    private int countLeft = 0;


    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            TrackGenerate();
        }
    }

    private void Update()
    {
        if (Turns.Count < 9 && Generate)
        {
            TrackGenerate();
        }

    }

    private void TrackGenerate()
    {
        Transform newRotate = Instantiate(Rotate[Random.Range(0, Rotate.Length)]);
        newRotate.position = Roads.Last().TransformPoint(Vector3.forward / distanceToTrack);
        newRotate.rotation = Roads.Last().rotation;
        Turns.Add(newRotate);

        
        if (Turns.Last().gameObject.tag == "Right")
        {
            countLeft = 0;
            countRight++;

            if (countRight > 2)
            {
                Generate = false;
                Destroy(Turns.Last().gameObject);
                Turns.Remove(Turns.Last().transform);
                generateLeft();
                Generate = true;
                countRight = 0;
            }
        }
        else
        {
            countRight = 0;
            countLeft++;

            if (countLeft > 2)
            {
                Generate = false;
                Destroy(Turns.Last().gameObject);
                Turns.Remove(Turns.Last().transform);
                generateRight();
                Generate = true;
                countLeft = 0;
            }
        }

        if (Turns.Last().gameObject.tag == "Right")
        {
            Transform newTrack = Instantiate(TrackPrefab);
            newTrack.position = Turns.Last().TransformPoint(Vector3.right / distanceToTurn);
            newTrack.rotation = Roads.Last().rotation;
            newTrack.Rotate(Vector3.up , 90);
            Roads.Add(newTrack);
        }
        else
        {
            Transform newTrack = Instantiate(TrackPrefab);
            newTrack.position = Turns.Last().TransformPoint(Vector3.left / distanceToTurn);
            newTrack.rotation = Roads.Last().rotation;
            newTrack.Rotate(Vector3.up , -90);
            Roads.Add(newTrack);
        }
    }

    private void generateRight()
    {
        countRight++;
        Transform newRotate = Instantiate(Rotate[1]);
        newRotate.position = Roads.Last().TransformPoint(Vector3.forward / distanceToTrack);
        newRotate.rotation = Roads.Last().rotation;
        Turns.Add(newRotate);
    }

    private void generateLeft()
    {
        countLeft++;
        Transform newRotate = Instantiate(Rotate[0]);
        newRotate.position = Roads.Last().TransformPoint(Vector3.forward / distanceToTrack);
        newRotate.rotation = Roads.Last().rotation;
        Turns.Add(newRotate);
    }

    public void DeletObj()
    {
        Destroy(Roads.First().transform.gameObject, 2f);
        Destroy(Turns.First().transform.gameObject, 2f);
        Roads.Remove(Roads.First().transform);
        Turns.Remove(Turns.First().transform);
    }


}
