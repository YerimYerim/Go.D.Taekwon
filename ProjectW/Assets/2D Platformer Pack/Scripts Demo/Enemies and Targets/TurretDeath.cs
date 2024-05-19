using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeath : MonoBehaviour
{
    public GameObject turret;
    public Animator turretAnim;
    [HideInInspector] public bool turretDead = false;

    private void OnCollisionEnter2D(Collision2D other)
    {

        OnCollision(other.gameObject);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        OnCollision(other.gameObject);

    }

    //Checks if turret is attacked by player
    private void OnCollision(GameObject collider)
    {
        if (collider.CompareTag("Weapon") || collider.CompareTag("Bullet"))
        {
            turretAnim.Play("Turret Death");
            turretDead = true;
            Destroy(turret, 1.5f);
        }
    }
}
