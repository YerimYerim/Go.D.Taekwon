using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectilePlayer : MonoBehaviour
{
    public GameObject collisionEffect;
    public GameObject collisionEffectEnemy;
    private float playerDirection;
    public float bulletLifeSpan;


    //Offset for hit effect spawn
    public Vector2 hitEffectOffset;

    private void Start()
    {
        Destroy(gameObject, bulletLifeSpan);

        //Flip direction of hit effect based on player localscale
        playerDirection = GameObject.Find("Player").transform.localScale.x;
        collisionEffect.transform.localScale = new Vector2(playerDirection, transform.localScale.y);
        collisionEffectEnemy.transform.localScale = new Vector2(playerDirection, transform.localScale.y);

    }


    public void OnCollisionEnter2D(Collision2D col)
    {

        Vector2 collisionPoint = col.contacts[0].point;

        if (col.gameObject.CompareTag("Enemy"))
        {
            GameObject hitEffect = Instantiate(collisionEffectEnemy, collisionPoint + (hitEffectOffset * playerDirection), Quaternion.identity);
            Animator anim = hitEffect.gameObject.GetComponent<Animator>();
            Destroy(hitEffect, anim.GetCurrentAnimatorStateInfo(0).length - 0.015f);
        }
        else
        {
            GameObject hitEffect2 = Instantiate(collisionEffect, collisionPoint + (hitEffectOffset * playerDirection), Quaternion.identity);
            Animator anim2 = hitEffect2.gameObject.GetComponent<Animator>();

            Destroy(hitEffect2, anim2.GetCurrentAnimatorStateInfo(0).length - 0.015f);
 
        }

        Destroy(gameObject);
     
    }

}
