using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Health")]
    public HealthBar healthBar;
    [HideInInspector] public bool dead;

    public int maxHP;
    public int currentHP;


    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();

        currentHP = maxHP;
        healthBar.SetMaxHP(maxHP);
    }

    void Update()
    {
        if (currentHP == 0)
        {

            dead = true;
            Death();
        }
        else if (currentHP < 0 )
        {
            currentHP = 0;
        }
    }

    public void Damage(int damageAmount)
    {
        currentHP -= damageAmount;

        healthBar.SetHP(currentHP);

    }

    private void Death()
    {

        playerController.playerAnimator.SetTrigger("Death");

        playerController.gameObject.tag = "Player Dead";

        //Ignores collision between player and projectile layers, re-enabled in Respawn method in SpawnManager script
        Physics2D.IgnoreLayerCollision(8, 9, true);

    }

}
