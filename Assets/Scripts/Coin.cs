using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
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
            anim.SetTrigger("CoinGet");
        }
    }


}
