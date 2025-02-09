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
    [SerializeField] private Transform rayPosition;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private AudioSource runAudio;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource landingAudio;
    [SerializeField] private AudioSource fallingAudio;
    [SerializeField] private AudioSource collisionAudio;
    [SerializeField] private AudioSource damageAudio;
    private LevelManager levelManager;
    private SpawnGenerate spawnGenerate;
    private ScoreManager scoreManager;
    private Animator anim;
    private Rigidbody _rb;
    private Vector3 touchStartPos;
    private bool moveRight;
    private bool moveLeft;
    private bool isGrounded;
    private int count = 0;
    private float timeToUpSpeed = 3f;
    private float maxTilt = 1f;
    private float rayRange = 0.44f;
    private float moveSpeed;


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
        CheckGround();
        SwipeMove();
        PlayerSpeedUp();

        moveSpeed = Mathf.Clamp(Input.acceleration.x, -maxTilt, maxTilt);

        if (transform.position.y < -17f)
        {
            scoreManager.CheckRecord();
            levelManager.onLoseMenu();
        }
    }


    private void FixedUpdate()
    {
        Move();

        cameraPos.position = Vector3.Lerp(cameraPos.position, transform.TransformPoint(0, 3.5f, -3.3f), 5f * Time.fixedDeltaTime);
        cameraPos.LookAt(CameraLookPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            Camera.main.GetComponentInChildren<AudioSource>().Pause();
            runAudio.Pause();
            collisionAudio.Play();
            scoreManager.CheckRecord();
            levelManager.onLoseMenu();
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

        if (other.tag == "Falling")
        {
            Camera.main.GetComponentInChildren<AudioSource>().Pause();
            fallingAudio.Play();
            anim.SetInteger("State", 2);
        }

        if (other.tag == "Bullet")
        {
            Camera.main.GetComponentInChildren<AudioSource>().Pause();
            runAudio.Pause();
            damageAudio.Play();
            scoreManager.CheckRecord();
            levelManager.onLoseMenu();
        }

        if (other.tag == "Coin")
        {
            scoreManager.UpPoints();
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

    private void CheckGround()
    {
        if (CheckStayOnGround())
        {
            if (!isGrounded)
            {
                isGrounded = true;
                landingAudio.Play();
                anim.SetInteger("State", 0);
                runAudio.Play();
            }
        }
        else
        {
            if (isGrounded)
            {
                isGrounded = false;
                runAudio.Pause();
            }
        }
    }

    private bool CheckStayOnGround()
    {
        return Physics.Raycast(rayPosition.position, Vector3.down, rayRange, groundMask);
    }

    private void PlayerSpeedUp()
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
    }

    private void Jump()
    {
        jumpAudio.Play();
        _rb.velocity = new Vector3(0f, 5.5f, 0f);
        anim.SetInteger("State", 1);
    }

    private void Move()
    {
        _rb.position += transform.right * 3f * moveSpeed * Time.fixedDeltaTime;
        _rb.position += transform.forward * speed * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rb.position += transform.right * 3f * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rb.position += transform.right * -3f * Time.fixedDeltaTime;
        }

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
