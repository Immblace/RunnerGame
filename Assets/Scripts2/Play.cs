using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private TextMeshProUGUI myScore;
    [SerializeField] private TextMeshProUGUI myRecord;
    [SerializeField] private Transform CameraLookPos;
    private SpawnGenerate spawnGenerate;
    private bool moveRight;
    private bool moveLeft;
    private int count = 0;
    private Vector3 startPos;
    private bool isGrounded;
    private Animator anim;
    private Rigidbody _rb;


    private float score = 0;
    private float timeToUpSpeed = 3f;

    private void Start()
    {
        spawnGenerate = FindObjectOfType<SpawnGenerate>();
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        myRecord.text = "Record: " + Math.Round(PlayerPrefs.GetFloat("MyRecord"));
    }

    void Update()
    {
        score += Time.deltaTime;
        myScore.text = "Score: " + Math.Round(score);

        if (timeToUpSpeed > 0)
        {
            timeToUpSpeed -= Time.deltaTime;
        }
        else
        {
            speed += 0.6f;
            timeToUpSpeed = UnityEngine.Random.Range(5, 9);
        }


        transform.position += transform.forward * speed * Time.deltaTime;
        SwipeMove();
        Move();

        if (transform.position.y < 0.048f)
        {
            anim.SetInteger("State", 0);
        }
        else
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rb.AddForce(Vector3.up * 0.02f , ForceMode.Impulse);
            anim.SetInteger("State", 1);
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

        if (transform.position.y < -20f)
        {
            if (score > PlayerPrefs.GetFloat("MyRecord"))
            {
                PlayerPrefs.SetFloat("MyRecord", score);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position = new Vector3(0f, 0.4f, -25f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void LateUpdate()
    {
        cameraPos.position = Vector3.Lerp(cameraPos.position, transform.TransformPoint(0, 3.5f, -3.3f), 5f * Time.deltaTime);
        cameraPos.LookAt(CameraLookPos);
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            if (score > PlayerPrefs.GetFloat("MyRecord"))
            {
                PlayerPrefs.SetFloat("MyRecord", score);
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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


    private void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 2f * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 2f * Time.deltaTime, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
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
                    startPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    float delta = touch.position.x - startPos.x;

                    if (Mathf.Abs(delta) > 5f)
                    {
                        if (delta > 0 && moveRight && count == 0)
                        {
                            transform.Rotate(Vector3.up, 90f);
                            count++;
                        }

                        if (delta < 0 && moveLeft && count == 0)
                        {
                            transform.Rotate(Vector3.up, -90f);
                            count++;
                        }
                    }
                    startPos = Vector3.zero;
                    break;
            }
        }
    }


}
