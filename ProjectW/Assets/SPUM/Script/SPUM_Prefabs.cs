using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPUM_Prefabs : MonoBehaviour
{
    private static readonly int RunState = Animator.StringToHash("RunState");
    private static readonly int Chk = Animator.StringToHash("EditChk");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackState = Animator.StringToHash("AttackState");
    private static readonly int NormalState = Animator.StringToHash("NormalState");
    public SPUM_SpriteList _spriteOBj;
    public bool EditChk;
    public string _code;
    public Animator _anim;

    public enum AnimationType
    {
        Idle = 0,
        Run = 1,
        Death = 2,
        Stun = 3,
        AttackSword = 4,
        AttackBow = 5,
        AttackMagic = 6,
        SkillSword = 7,
        SkillBow = 8,
        SkillMagic = 9
    }
    public void PlayAnimation(AnimationType animationType)
    {
        switch(animationType)
        {
            case AnimationType.Idle: //Idle
            _anim.SetFloat(RunState,0f);
            break;

            case AnimationType.Run: //Run
            _anim.SetFloat(RunState,0.5f);
            break;

            case AnimationType.Death: //Death
            _anim.SetTrigger(Die);
            _anim.SetBool(Chk,EditChk);
            break;

            case AnimationType.Stun: //Stun
            _anim.SetFloat(RunState,1.0f);
            break;

            case AnimationType.AttackSword: //Attack Sword
            _anim.SetTrigger(Attack);
            _anim.SetFloat(AttackState,0.0f);
            _anim.SetFloat(NormalState,0.0f);
            break;

            case AnimationType.AttackBow: //Attack Bow
            _anim.SetTrigger(Attack);
            _anim.SetFloat(AttackState,0.0f);
            _anim.SetFloat(NormalState,0.5f);
            break;

            case AnimationType.AttackMagic: //Attack Magic
            _anim.SetTrigger(Attack);
            _anim.SetFloat(AttackState,0.0f);
            _anim.SetFloat(NormalState,1.0f);
            break;

            case AnimationType.SkillSword: //Skill Sword
            _anim.SetTrigger(Attack);
            _anim.SetFloat(AttackState,1.0f);
            _anim.SetFloat(NormalState,0.0f);
            break;

            case AnimationType.SkillBow: //Skill Bow
            _anim.SetTrigger(Attack);
            _anim.SetFloat(AttackState,1.0f);
            _anim.SetFloat(NormalState,0.5f);
            break;

            case AnimationType.SkillMagic: //Skill Magic
            _anim.SetTrigger(Attack);
            _anim.SetFloat(AttackState,1.0f);
            _anim.SetFloat(NormalState,1.0f);
            break;
        }
    }
}
