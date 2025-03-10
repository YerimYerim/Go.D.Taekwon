using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorDataBase
{
    public int Hp { get; private set; }
    public int MaxHp { get; private set; }

    protected AmorStat amor = new();
    protected DamageTakeStat damageTakeStat = new(0);
    protected AttackStat attackStat = new();
    protected bool ignoreDamage = false;
    
    public List<SkillEffectBase> turnSkill = new() ;

    public event Action</* damage */int> OnEventDamaged;
    public event Action</*totalHeal*/int, /*overHeal*/ int> OnHeal;
    public event Action</* damage */int> OnGiveDamage;
    
    //Enemy 의 경우에만 사용

    // - 대미지 = 피해량 - (방어도 * (1-방어도 계수 파괴)  - 방어도 정수 파괴)
    // - 피해량 = (스킬 공격력 + 피해 정수 증폭) * (1+피해 계수 증폭) * (1-대상 받는피해량 계수 증감)
    // - 회복공식 = 회복량 * (1+회복 계수 증폭)
    public void Init(int hp)    
    {
        Hp = hp;
        MaxHp = hp;
        
        InitAmorStat(0, 0, 0f, 0f);
        InitAttackStat(0);
        InitTakeDamageStat(0);
        SetTakeDamageStat(0);
    }
    public void TakeDamage(int damage, int trueDamage = 0)
    {
        // 남은 데미지
        if (ignoreDamage == true)
        {
            var ignoreSkill = turnSkill.Find(_ => _ is ISkillIgnoreDamage);
            
            if (ignoreSkill is ISkillStackable stackableIgnoreSkill)
            {
                stackableIgnoreSkill.RemoveStack();
            }
            return;
        } 

        var leftDamage = Mathf.CeilToInt(amor.amor - (amor.GetFinalDamage(damage) * (1f + damageTakeStat.takeDamage * 0.01f)));
        
        if(leftDamage < 0)
        {
            Hp = Mathf.CeilToInt(Mathf.Max(0f, Hp + (leftDamage)));
        }
        else
        {
            amor.amor = Math.Max(0, leftDamage);
        }
        Hp = Mathf.Max(0, Hp - trueDamage);
        
        OnEventDamaged?.Invoke(leftDamage);
    }
    
    public void GiveDamage(int damage)
    {
        OnGiveDamage?.Invoke(damage);
    }
    
    public void DoHeal(int addHp)
    { 
        Hp = Mathf.Min(Hp + addHp, MaxHp);
        var overHeal = Mathf.Max(0, Hp + addHp - MaxHp);
        OnHeal?.Invoke(addHp, overHeal);
    }

    public int GetHp()
    {
        return Hp;
    }    
    public int GetMaxHp()
    {
        return MaxHp;
    }

    public void InitAmorStat(int amor, int amorBreakInt, float amorBreakper, float takedDamage)
    {
        this.amor = new()
        {
            amor = amor, // 방어도
            amorBreakInt = amorBreakInt,
            amorBreakPercent = amorBreakper,
            takedamage = takedDamage
        };
    }    
    
    public void InitAttackStat( int damageUpInt)
    {
        attackStat = new();
    }
    public void InitTakeDamageStat( int damageUpInt)
    {
        damageTakeStat = new(damageUpInt);
    }
    public void SetTakeDamageStat(int takeDamage)
    {
       damageTakeStat.SetDamageStat(takeDamage);
    }    
    public void SetAttackStat(int stat)
    { 
        attackStat.SetAttackStat(stat);
    }    
    public void SetFixedDamageStat(int fixedDamage)
    { 
        attackStat.SetFixedAttackStat(fixedDamage);
    }       
    
    public void SetTrueDamage(int trueDamage)
    { 
        attackStat.SetTrueDamage(trueDamage);
    }    
    
    public AttackStat GetAttackStat()
    {
        return attackStat;
    }
    
    public void AddAmorStat(int amorStatValue)
    {
        amor.AddAmor(amorStatValue);
    }

    public void AddTurnSkill(SkillEffectBase skill) 
    {
        turnSkill.Add(skill);
    }

    public int GetAmor()
    {
        return amor.amor;
    }
    
    public bool IsIgnoreDamage()
    {
        return ignoreDamage;
    }
    
    public void SetIgnoreDamage(bool ignore)
    {
        ignoreDamage = ignore;
    }
}
