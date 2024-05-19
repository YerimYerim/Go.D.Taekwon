using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    public int damageAmount;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !playerController.isBlocking)
        {
            playerHealth.Damage(damageAmount);

        }
    }

}
