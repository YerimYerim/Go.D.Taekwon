using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    PlayerController playerController;
    PlayerHealth playerHealth;
    public GameObject spawnPoint;
    public float respawnWaitTime;

    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();

        //Spawns player at "Start Here" object in scene view 
        playerController.transform.position = spawnPoint.transform.position;

        //Disables the "Start Here" sprite on awake
        spawnPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        playerHealth = gameObject.GetComponent<PlayerHealth>();

        playerHealth.dead = false;

    }

    private void Update()
    {
        if (playerHealth.dead)
        {
            StartCoroutine(RespawnDelay());
        }
   
    }


    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSecondsRealtime(respawnWaitTime);
        Respawn();
        playerHealth.dead = false;

    }
    private void Respawn()
    {
        //Re enables functions disabled on death
        if (playerHealth.dead)
        {
            playerController.gameObject.tag = "Player";
            playerController.playerAnimator.ResetTrigger("Death");
            playerController.transform.position = spawnPoint.transform.position;
            playerController.MovementResume();
            playerController.Return2Idle();
            playerHealth.currentHP = playerHealth.maxHP;
            playerHealth.healthBar.SetHP(playerHealth.currentHP);

            Physics2D.IgnoreLayerCollision(8, 9, false);
        }

    }

}
