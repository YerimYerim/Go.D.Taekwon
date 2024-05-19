using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    private PlayerController playerController;
    private Animator scarecrowAnim;
    public GameObject attackTutorialObject;
    private AttackTutorial attackTutorial;
    private bool nextAttackTutorial = false;

    void Start()
    {
        scarecrowAnim = gameObject.GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        attackTutorial = attackTutorialObject.GetComponent<AttackTutorial>();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon") && !nextAttackTutorial)
        {
            scarecrowAnim.Play("Scarecrow Hit");

            FindObjectOfType<HitStop>().Stop();

        }

        if (other.gameObject.CompareTag("Weapon") && nextAttackTutorial)
        {
            nextAttackTutorial = false;

            attackTutorial.triggerEvent2 = false;

            scarecrowAnim.Play("Scarecrow Death");
            FindObjectOfType<HitStop>().HardStop();


            AttacksEnable();

            Destroy(attackTutorialObject);

            Destroy(gameObject, 1f);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            nextAttackTutorial = true;
        }
    }

    private void AttacksEnable()
    {
        
        playerController.MovementResume();

        playerController.canAttack = true;
        playerController.canChargeAttack = true;

    }

}
