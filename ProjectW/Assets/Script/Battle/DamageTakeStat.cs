using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 받는 피해량 증감 관련 처리
public record DamageTakeStat
{
    public int takeDamage { get; private set; }

    public DamageTakeStat(int takeDamage)
    {
        this.takeDamage = 0;
    }
    
    public void SetDamageStat(int takeDamage)
    {
        this.takeDamage += takeDamage;
    }
    
    public void AddTakeDamage(int damage)
    {
        takeDamage += damage;
    }

    public void MinusTakeDamage(int damage)
    {
        takeDamage -= damage;
    }
}
