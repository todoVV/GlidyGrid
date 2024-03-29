using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    private GameObject player;

    [Header("SFX")] 
    [SerializeField] private AudioClip heartPickup;
    [SerializeField] private AudioClip coinPickup;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Health"))
            {
                if (player.GetComponent<PlayerController>().isNotFullHealth)
                {
                    player.GetComponent<PlayerController>().StartCoroutine("GainHeart");
                    SFXController.instance.PlaySFX(heartPickup, transform, 0.05f);
                }
                else
                {
                    return;
                }
            }
            else if (gameObject.CompareTag("Coin"))
            {
                player.GetComponent<PlayerController>().StartCoroutine("GainCoin");
                SFXController.instance.PlaySFX(coinPickup, transform, 0.05f);
            }
            Destroy(gameObject);
        }
    }
}
