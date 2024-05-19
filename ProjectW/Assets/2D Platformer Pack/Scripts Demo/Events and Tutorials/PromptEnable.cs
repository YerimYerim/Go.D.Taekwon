using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptEnable : MonoBehaviour
{

    [SerializeField] GameObject prompt;
    private Animator promptAnim;


    void Start()
    {
        promptAnim = prompt.GetComponent<Animator>();

    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptAnim.Play("Attack Prompt In");

            StartCoroutine(Wait());

        }

    }

    IEnumerator Wait()
    {

        yield return new WaitUntil(() => InputManager.XButtonDown());
        promptAnim.Play("Attack Prompt Out");

        Destroy(gameObject, 1);
    }

}
