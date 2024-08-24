using System;
using System.Collections.Generic;

public class ActorDataBase
{
    public int Hp { get; private set; }
    public int MaxHp { get; private set; }

    protected AmorStat amor = new();
    protected AttackStat _attackStat = new();
    protected HealStat _healStat = new();
    protected AvoidStat _avoidStat = new();

    public LinkedList<ISkillTargetDebuff> debuffs = new();
    public LinkedList<ISkillTargetBuff> buffs = new();
    
    private bool IsStun = false;

    //Enemy 의 경우에만 사용

    // - 대미지 = 피해량 - (방어도 * (1-방어도 계수 파괴)  - 방어도 정수 파괴)
    // - 피해량 = (스킬 공격력 + 피해 정수 증폭) * (1+피해 계수 증폭) * (1-대상 받는피해량 계수 증감)
    // - 회복공식 = 회복량 * (1+회복 계수 증폭)
    // - 명중 체크 = 100*(1 - 회피율)%, (min=30%)
    public void Init(int hp)    
    {
        Hp = hp;
        MaxHp = hp;
        InitAmorStat(0, 0, 0f, 0f);
        InitAttackStat(0, 0, 0f);
    }
    public void DoDamaged(int damage)
    {
        var leftDamage = amor.amor - Math.Max(0, amor.GetFinalDamage(damage));
        amor.AddAmor(  Math.Max(0, amor.GetFinalDamage(damage)) );
        Hp -= Math.Max(0, leftDamage);
    }
    
    public void DoHeal(int addHp)
    {
        Hp += addHp;
    }

    public int GetHp()
    {
        return Hp;
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
    
    public void InitAttackStat(int skillDamage, int damageUpInt, float damageUpPer)
    {
        _attackStat = new()
        {
            skillDamage = skillDamage,
            damageUpInt = damageUpInt,
            damageUpPercent = damageUpPer,
        };
    }

    public void AddAmorStat(int amorStatValue)
    {
        amor.AddAmor(amorStatValue);
    }

    public void AddBuff(ISkillTargetBuff buff)
    {
        buffs.AddLast(buff);
    }

    public void AddDebuff(ISkillTargetDebuff debuff)
    {
        debuffs.AddLast(debuff);
    }

    public int GetAmor()
    {
        return amor.amor;
    }
}
