using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 startPos;


    private void Update()
    {
        MoveMouse();
        SwipeMove();
    }

    private void SwipeMove()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Canceled:
                    float deltaX = touch.position.x - startPos.x;

                    if (Mathf.Abs(deltaX) > 5f)
                    {
                        if (deltaX > 0)
                        {
                            if (transform.position.x < 1.75f)
                            {
                                transform.position += Vector3.right * 1.8f;
                            }
                        }
                        else
                        {
                            if (transform.position.x > -1.75f)
                            {
                                transform.position += Vector3.left * 1.8f;
                            }
                        }
                    }

                    startPos = Vector3.zero;
                    break;
            }
        }
    }


    private void MoveMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float deltaX = Input.mousePosition.x - startPos.x;

            if (Mathf.Abs(deltaX) > 5f)
            {
                if (deltaX > 0)
                {
                    if (transform.position.x < 1.75f)
                    {
                        transform.position += Vector3.right * 1.8f;
                    }
                }
                else
                {
                    if (transform.position.x > -1.75f)
                    {
                        transform.position += Vector3.left * 1.8f;
                    }
                }
            }

            startPos = Vector3.zero;
        }
    }
}
