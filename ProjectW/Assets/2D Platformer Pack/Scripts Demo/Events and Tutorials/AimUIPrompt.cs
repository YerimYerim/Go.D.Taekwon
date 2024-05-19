using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUIPrompt : MonoBehaviour
{
    public GameObject promptObj;
    private Animator animPrompt;
    private PlayerController playerController;
    private Collider2D col;
    private bool canClosePrompt;

    void Start()
    {
        animPrompt = promptObj.GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        col = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if(canClosePrompt)
        {
            if(Input.anyKeyDown)
            {
                StartCoroutine(WaitForMovementResume());

                animPrompt.Play("360 Aim Prompt Out");

                Destroy(gameObject, 3);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerController.MovementStop();

            animPrompt.Play("360 Aim Prompt In");

            col.enabled = false;    
            
            StartCoroutine(Prompt());

        }

    }

    private IEnumerator Prompt()
    {

        yield return new WaitForSeconds(1.5f);

        canClosePrompt = true;
    }

    private IEnumerator WaitForMovementResume()
    {
        yield return new WaitForSeconds(0.3f);
        playerController.MovementResume();
    }

}
