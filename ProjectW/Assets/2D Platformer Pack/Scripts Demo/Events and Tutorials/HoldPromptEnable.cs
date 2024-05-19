using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPromptEnable : MonoBehaviour
{

    [SerializeField] GameObject prompt;
    private Animator promptAnim;
    private AttackTutorial attackTutorial;
    private PlayerController playerController;


    void Start()
    {
        promptAnim = prompt.GetComponent<Animator>();

        attackTutorial = GameObject.Find("Attack Tutorial Event").GetComponent<AttackTutorial>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.attackInitiated)
        {
            promptAnim.Play("Attack Hold Prompt Out");
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            StartCoroutine(HoldAttack());
        }

    }


    IEnumerator HoldAttack()
    {

        yield return new WaitUntil(() => attackTutorial.triggerPrompt);
        yield return new WaitForSeconds(2);

        attackTutorial.triggerPrompt = false;

        promptAnim.Play("Attack Hold Prompt");

        Destroy(gameObject, 7f);

    }
}
