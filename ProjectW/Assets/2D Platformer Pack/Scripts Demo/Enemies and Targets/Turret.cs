using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool turretActive;
    private float playbackMultiplier;
    public Animator turretAnim;
    public Transform detectionMarker;
    public GameObject bulletPrefab;
    public Transform bulletOrigin;

    public TurretDeath turretDeath;

    [Space(30)]

    public float detectionRadius;
    public float fireRate;
    public float projectileSpeed;
    private bool canFire = true;

    private void OnValidate()
    {
        //Controls collider radius and detection marker in inspector through the detection radius value 
        gameObject.GetComponent<CircleCollider2D>().radius = detectionRadius;

        detectionMarker.transform.localScale = new Vector3(detectionRadius, 1, 1);

    }


    //Controls the activation animations for the turret playing forwards and backwards when player enters range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!turretDeath.turretDead)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                playbackMultiplier = 1f;
                TurretActivation();

                if (!turretAnim.GetCurrentAnimatorStateInfo(0).IsName("Turret Activation Reverse"))
                {
                    turretAnim.Play("Turret Activation");
                }

            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!turretDeath.turretDead)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player Dead"))
            {
                playbackMultiplier = -1f;
                TurretActivation();
                if (!turretAnim.GetCurrentAnimatorStateInfo(0).IsName("Turret Activation"))
                {
                    turretAnim.Play("Turret Activation Reverse");
                }

            }
        }

    }


    private void TurretActivation()
    {
        turretAnim.SetFloat("Reverse", playbackMultiplier);
        turretAnim.SetFloat("Reverse Deactivate", -playbackMultiplier);
    }


    private void Update()
    {


        if (turretAnim.GetCurrentAnimatorStateInfo(0).IsName("Turret Static"))
        {
            turretActive = true;
        }

        else
        {
            turretActive = false;

        }

        if (turretActive && canFire)
        {

            StartCoroutine(Fire());

        }

    }


    private IEnumerator Fire()
    {
        canFire = false;

        var bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);

        bullet.transform.localScale = new Vector3(transform.localScale.x, 1, 1);

        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(projectileSpeed * transform.localScale.x * 10, 0), ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(1 / fireRate);

        canFire = true;

    }

}
