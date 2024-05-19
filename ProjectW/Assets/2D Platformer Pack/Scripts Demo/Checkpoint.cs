using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject spawnPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var anim = gameObject.GetComponent<Animator>();
            anim.Play("Contact");

            var collider = gameObject.GetComponent<Collider2D>();

            collider.enabled = false;


            //Spawns player at last checkpoint activated
            spawnPoint.transform.position = gameObject.transform.position;

        }
    }
}
