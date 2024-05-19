using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrail : MonoBehaviour
{
    private PlayerController playerController;
    private TrailRenderer trail;


    // Start is called before the first frame update
    void Start()
    {
 
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        trail = gameObject.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.IsGrounded())
        {
            trail.enabled = true;
            trail.emitting = false;
            StopAllCoroutines();
        }
        else if (!playerController.falling)
        {
            trail.emitting = true;
            StartCoroutine(TrailIntermitent());

            StartCoroutine(TrailTime());

        }
 

    }

    private IEnumerator TrailIntermitent()
    {
        yield return new WaitForSeconds(0.03f);
        trail.emitting = false;
    }

    private IEnumerator TrailTime()
    {
        yield return new WaitForSeconds(0.25f);
        trail.enabled = false;
    }
}
