using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileEnemy : MonoBehaviour
{
    public GameObject collisionObject;
    private float enemyDirection;
    public float bulletLifeSpan;


    //Offset for hit effect spawn
    public Vector2 hitEffectOffset;

    private void Start()
    {
        Destroy(gameObject, bulletLifeSpan);

        enemyDirection = gameObject.transform.localScale.x;


    }

    //Spawns dust animation when projectile hits something
    public void OnCollisionEnter2D(Collision2D col)
    {

        Vector2 collisionPoint = col.contacts[0].point;

        GameObject hitEffect = Instantiate(collisionObject, collisionPoint + (hitEffectOffset * enemyDirection), Quaternion.identity);

        Destroy(gameObject);

        Destroy(hitEffect, 0.36f);

    }

}
