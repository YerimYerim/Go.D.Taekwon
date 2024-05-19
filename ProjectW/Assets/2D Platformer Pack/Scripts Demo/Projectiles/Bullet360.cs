using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet360 : MonoBehaviour
{
    private float playerDirection;
    public GameObject collisionObject;
    public float bulletLifeSpan;
    private LaserSight laserSight;


    //Offset for hit effect spawn
    public Vector2 hitEffectOffset;

    private void Start()
    {
        Destroy(gameObject, bulletLifeSpan);
        laserSight = GameObject.Find("Laser Renderer").GetComponent<LaserSight>();

        //Flip direction of hit effect based on player localscale
        playerDirection = GameObject.Find("Player").transform.localScale.x;
        collisionObject.transform.localScale = new Vector2(transform.localScale.x * playerDirection, transform.localScale.y);

    }


    public void OnCollisionEnter2D(Collision2D col)
    {

        Vector2 collisionPoint = col.contacts[0].point;

        Instantiate(collisionObject, collisionPoint + hitEffectOffset, laserSight.aimPivot.gameObject.transform.rotation);

        Destroy(gameObject);

    }
}
