using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private AudioSource audioSource;
    private Animator animation;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animator>();
    }

    public void DeleteCoin()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.Play();
            animation.SetTrigger("CoinGet");
        }
    }


}
