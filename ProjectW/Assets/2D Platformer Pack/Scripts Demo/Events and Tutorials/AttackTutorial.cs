using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorial : MonoBehaviour
{

    private PlayerController playerController;

    [HideInInspector] public bool triggerPrompt = false;
    private bool triggerEvent = false;
    [HideInInspector] public bool triggerEvent2 = false;



    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    void Update()
    {
        if (InputManager.XButtonDown() && triggerEvent)
        {
  
            playerController.Attack();
            triggerEvent = false;
            triggerEvent2 = true;
            StartCoroutine(ChargeAttackEnable());
        }

        if(InputManager.XButtonHold() && playerController.canChargeAttack && triggerEvent2)
        {
            playerController.attackInitiated = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            playerController.MovementStop();
            triggerEvent = true;

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerController.IsGrounded())
            {
                playerController.MovementStop();
            }
        }
    }


    private IEnumerator ChargeAttackEnable()
    {
        triggerPrompt = true;
        yield return new WaitForSeconds(2);
        playerController.canChargeAttack = true;
    }

}
