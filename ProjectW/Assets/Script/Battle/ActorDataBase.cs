using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorDataBase
{
    public int Hp { get; private set; }
    protected int _deffense;

    //Enemy 의 경우에만 사용
    public int ActionPoint { get; private set; }
    // - 대미지 = 피해량 - (방어도 * (1-방어도 계수 파괴)  - 방어도 정수 파괴)
    // - 피해량 = (스킬 공격력 + 피해 정수 증폭) * (1+피해 계수 증폭) * (1-대상 받는피해량 계수 증감)
    // - 회복공식 = 회복량 * (1+회복 계수 증폭)
    // - 명중 체크 = 100*(1 - 회피율)%, (min=30%)
    public void Init(int hp, int AP = 0)
    {
        Hp = hp;
        ActionPoint = AP;
    }
    public void DoDamaged(int damage)
    {
        Hp -= damage;
    }
    
    public void DoHeal(int addHp)
    {
        Hp += addHp;
    }

    public int GetHp()
    {
        return Hp;
    }    
    
    public int GetAP()
    {
        return ActionPoint;
    }
    
    public void MinusAP(int addAp)
    {
        ActionPoint -= addAp;
    }
    public void ResetAP(int ap)
    {
        ActionPoint = ap;
    }
}