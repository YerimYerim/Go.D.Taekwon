using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 받는 피해량 증감 관련 처리
public record TakeDamageStat
{
    public int takeDamage { get; private set; }
    public int takeTurn { get; private set; }
    
    
    public TakeDamageStat(int takeDamage, int takeTurn)
    {
        this.takeDamage = 100 + takeDamage;
        this.takeTurn = takeTurn;
    }
    public void SetDamageStat(int takeDamage, int takeTurn)
    {
        this.takeDamage *= takeDamage;
        this.takeTurn = takeTurn;
    }
    public void AddTakeDamage(int damage)
    {
        takeDamage *= damage;
    }
    
    private bool IsCanApply()
    {
        return takeTurn > 0;
    }
    
    public float GetTakeDamage()
    {
        return IsCanApply() ? 1f : takeDamage * 0.01f;
    }
}
