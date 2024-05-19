using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClip : MonoBehaviour
{
    private AnimationClip[] clips;
    private Animator animator;

    private void Start()
    {
       
        animator = GetComponent<Animator>();
        clips = animator.runtimeAnimatorController.animationClips;

        StartCoroutine(Randomize());

    }

    private IEnumerator Randomize()
    {
        var index = Random.Range(0, clips.Length);

        var randomClip = clips[index];

        animator.Play(randomClip.name);

        yield return new WaitForSeconds(randomClip.length);

        Destroy(gameObject);
    }
}
