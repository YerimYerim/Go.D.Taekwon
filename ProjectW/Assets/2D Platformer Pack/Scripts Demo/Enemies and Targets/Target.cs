using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator targetAnim;

    private void Start()
    {
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
        FindAnyObjectByType<HitStop>().Stop();

        gameObject.GetComponent<Collider2D>().enabled = false;

        targetAnim.Play("Target Hit");

        var currentClip = targetAnim.runtimeAnimatorController.animationClips;

        Destroy(gameObject, currentClip[0].length);
    }

}
