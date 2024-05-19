using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSwitch : MonoBehaviour
{
    //This script goes on a target that acts as a switch when destroyed

    //Drag in which wall will open upon destroying target
    public GameObject wall;

    private WallInteraction wallInteraction;

    private Animator targetAnim;

    private void Start()
    {
        wallInteraction = wall.GetComponent<WallInteraction>();
        targetAnim = gameObject.GetComponent<Animator>();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TargetDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            TargetDestroy();
        }
    }


    private void TargetDestroy()
    {
        wallInteraction.targetsRemaining.Remove(gameObject);
        
        FindAnyObjectByType<HitStop>().Stop();

        targetAnim.Play("Target Hit");

        var currentClip = targetAnim.runtimeAnimatorController.animationClips;

        Destroy(gameObject, currentClip[0].length);
    }



}
