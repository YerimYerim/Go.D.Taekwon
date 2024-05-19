using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private PlayerController playerController;
    Animator chestAnim;

    [Header("Chest Contents")]
    public bool enableGunAttacks;
    public bool enableMobilityAbilities;

    private bool triggerEvent = false;


    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        chestAnim = gameObject.GetComponent<Animator>();

    }

    void Update()
    {
        if (InputManager.BButtonDown() && triggerEvent)
        {
            playerController.playerAnimator.Play("Opening Chest");
            StartCoroutine(OpenChest());
            triggerEvent = false;
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

    private IEnumerator OpenChest()
    {

        if (enableGunAttacks)
        {
            yield return new WaitForSeconds(0.55f);
            chestAnim.Play("ChestObject");
        }

        if (enableMobilityAbilities)
        {
            if (playerController.weapon == 2)
            {   
                //Puts gun away if equipped
                playerController.WeaponSwap();

                yield return new WaitForSeconds(0.35f);

                playerController.playerAnimator.Play("Opening Chest");

                yield return new WaitForSeconds(0.55f);

                chestAnim.Play("ChestObject2");
            }
            else
            {
                yield return new WaitForSeconds(0.55f);
                chestAnim.Play("ChestObject2");
            }
        }

        var currentClip = chestAnim.runtimeAnimatorController.animationClips;

        yield return new WaitForSeconds(currentClip[0].length);

        if (enableGunAttacks)
        {
            playerController.canWeaponSwap = true;
            playerController.canFire = true;
            playerController.canFireCrouching = true;
            playerController.canFreeAim = true;
        }

        if (enableMobilityAbilities)
        {
            playerController.canDoubleJump = true;
            playerController.canWallJump = true;
            playerController.canWallSlide = true;
        }


        Destroy(gameObject);
        playerController.MovementResume();

        
    }

}
