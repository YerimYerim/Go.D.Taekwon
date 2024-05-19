using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptDisable : MonoBehaviour
{

    [SerializeField] GameObject prompt;
    private Animator promptAnim;


    // Start is called before the first frame update
    void Start()
    {
        promptAnim = prompt.GetComponent<Animator>();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptAnim.Play("FadePromptRun");
            Destroy(gameObject, 2);

        }
    }


}
