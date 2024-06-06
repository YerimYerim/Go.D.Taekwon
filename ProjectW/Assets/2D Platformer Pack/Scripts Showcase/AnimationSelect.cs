using UnityEngine;

public class AnimationSelect : MonoBehaviour
{
    public Animator animator;
    public string anim;

    private void Update()
    {
        animator.Play(anim);
    }

}
