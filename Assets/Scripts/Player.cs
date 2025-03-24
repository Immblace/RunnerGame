using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button TurnButton;
    private SpawnGenerate spawnGenerate;
    private ScoreManager scoreManager;
    private Animator anim;
    private Rigidbody _rb;
    private Turns turns;
    private Vector3 touchStartPos;
    private bool moveRight;
    private bool moveLeft;
    private bool isGrounded;
    private float timeToUpSpeed = 3f;
    private float rayRange = 0.44f;
    private int PlayerOffset;
    //private float maxTilt = 1f;
    //private float tiltSpeed;


    private void Start()
    {
        spawnGenerate = FindFirstObjectByType<SpawnGenerate>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();
        SwipeMove();
        PlayerSpeedUp();

        if (transform.position.y < -17f)
        {
            scoreManager.CheckRecord();
            scoreManager.onLoseMenu();
        }

        //tiltSpeed = Mathf.Clamp(Input.acceleration.x, -maxTilt, maxTilt);
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
            scoreManager.onLoseMenu();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Right")
        {
            turns = other.GetComponent<Turns>();
            moveRight = true;
            TurnButton.interactable = true;
        }

        if (other.tag == "Left")
        {
            turns = other.GetComponent<Turns>();
            moveLeft = true;
            TurnButton.interactable = true;
        }

        if (other.tag == "Falling")
        {
            Camera.main.GetComponentInChildren<AudioSource>().Pause();
            fallingAudio.Play();
            anim.SetInteger("State", (int)States.falling);
        }

        if (other.tag == "Bullet")
        {
            Camera.main.GetComponentInChildren<AudioSource>().Pause();
            runAudio.Pause();
            damageAudio.Play();
            scoreManager.CheckRecord();
            scoreManager.onLoseMenu();
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
                anim.SetInteger("State", (int)States.run);
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
            timeToUpSpeed = UnityEngine.Random.Range(3, 6);
        }
    }

    private void Jump()
    {
        jumpAudio.Play();
        _rb.linearVelocity = new Vector3(0f, 5.5f, 0f);
        anim.SetInteger("State", (int)States.jump);
    }

    private void Move()
    {
        //_rb.position += transform.right * 6f * tiltSpeed * Time.fixedDeltaTime;
        _rb.position += transform.right * 2.5f * PlayerOffset * Time.fixedDeltaTime;
        _rb.position += transform.forward * speed * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rb.position += transform.right * 3f * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rb.position += transform.right * -3f * Time.fixedDeltaTime;
        }

        if (moveRight)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveRight = false;
                turns.DestroyTurnWall();
                transform.Rotate(Vector3.up, 90f);
            }
        }

        if (moveLeft)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveLeft = false;
                turns.DestroyTurnWall();
                transform.Rotate(Vector3.up, -90f);
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
                            if (delta.x > 0 && moveRight)
                            {
                                moveRight = false;
                                turns.DestroyTurnWall();
                                transform.Rotate(Vector3.up, 90f);
                            }

                            if (delta.x < 0 && moveLeft)
                            {
                                moveLeft = false;
                                turns.DestroyTurnWall();
                                transform.Rotate(Vector3.up, -90f);
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

    public void PlayerOffsetRight()
    {
        PlayerOffset = 1;
    }

    public void PlayerOffsetLeft()
    {
        PlayerOffset = -1;
    }

    public void PlayerBtnUp()
    {
        PlayerOffset = 0;
    }

    public void TurnPlayer()
    {
        if (turns.gameObject.tag == "Right" && moveRight)
        {
            TurnButton.interactable = false;
            moveRight = false;
            turns.DestroyTurnWall();
            transform.position = new Vector3(turns.GetPlayerPos().position.x, transform.position.y, turns.GetPlayerPos().position.z);
            transform.Rotate(Vector3.up, 90f);
        }
        else if (turns.gameObject.tag == "Left" && moveLeft)
        {
            TurnButton.interactable = false;
            moveLeft = false;
            turns.DestroyTurnWall();
            transform.position = new Vector3(turns.GetPlayerPos().position.x, transform.position.y, turns.GetPlayerPos().position.z);
            transform.Rotate(Vector3.up, -90f);
        }
    }

    private enum States
    {
        run,
        jump,
        falling
    }

}
