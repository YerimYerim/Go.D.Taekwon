using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AnimationSelect : MonoBehaviour
{
    public Animator animator;
    public string anim;

    private void Update()
    {
        animator.Play(anim);
    }

}
