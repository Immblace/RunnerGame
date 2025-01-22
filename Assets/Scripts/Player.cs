using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform CameraLookPos;
    private LevelManager levelManager;
    private SpawnGenerate spawnGenerate;
    private ScoreManager scoreManager;
    private bool moveRight;
    private bool moveLeft;
    private int count = 0;
    private Vector3 touchStartPos;
    private bool isGrounded;
    private Animator anim;
    private Rigidbody _rb;
    private float timeToUpSpeed = 3f;


    private void Start()
    {
        spawnGenerate = FindObjectOfType<SpawnGenerate>();
        levelManager = FindObjectOfType<LevelManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (timeToUpSpeed > 0)
        {
            timeToUpSpeed -= Time.deltaTime;
        }
        else
        {
            speed += 0.6f;
            timeToUpSpeed = UnityEngine.Random.Range(5, 9);
        }

        SwipeMove();
        


        if (moveRight && count == 0)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.Rotate(Vector3.up, 90f);
                count++;
            }
        }

        if (moveLeft && count == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Rotate(Vector3.up, -90f);
                count++;
            }
        }

        if (transform.position.y < -20f)
        {
            scoreManager.CheckRecord();
            levelManager.onLoseMenu();
            transform.position = new Vector3(0f, 0.4f, -25f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        Move();

        cameraPos.position = Vector3.Lerp(cameraPos.position, transform.TransformPoint(0, 3.5f, -3.3f), 5f * Time.fixedDeltaTime);
        cameraPos.LookAt(CameraLookPos);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            scoreManager.CheckRecord();
            levelManager.onLoseMenu();
        }

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetInteger("State", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Right")
        {
            moveRight = true;
        }

        if (other.tag == "Left")
        {
            moveLeft = true;
        }

        if (other.tag == "Bullet")
        {
            scoreManager.CheckRecord();
            levelManager.onLoseMenu();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Right" || other.tag == "Left")
        {
            spawnGenerate.DeletObj();
            moveLeft = false;
            moveRight = false;
            count = 0;
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        anim.SetInteger("State", 1);
    }

    private void Move()
    {
        _rb.position += transform.forward * speed * Time.fixedDeltaTime;


        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rb.position += transform.right * 3f * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rb.position += transform.right * -3f * Time.fixedDeltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void SwipeMove()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    Vector3 delta = (Vector3)touch.position - touchStartPos;
                    
                    if (delta.magnitude > 200f)
                    {
                        if (Math.Abs(delta.x) > Math.Abs(delta.y))
                        {
                            if (delta.x > 0 && moveRight && count == 0)
                            {
                                transform.Rotate(Vector3.up, 90f);
                                count++;
                            }

                            if (delta.x < 0 && moveLeft && count == 0)
                            {
                                transform.Rotate(Vector3.up, -90f);
                                count++;
                            }
                        }
                        else
                        {
                            if (delta.y > 0 && isGrounded)
                            {
                                Jump();
                            }
                        }
                    }


                    touchStartPos = Vector3.zero;
                    break;
            }
        }
    }

}
