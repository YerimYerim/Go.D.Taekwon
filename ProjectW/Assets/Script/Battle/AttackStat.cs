using System;

public record AttackStat
{
    private int _damageTrue;
    public int TrueDamageUp
    {
        get => Math.Max(0, _damageTrue);
        private set => _damageTrue = value;
    }    
    
    public int DamageUpPer { get; private set; }
    public int DamageUpFixed { get; private set; }
    
    public void SetAttackStat(int takeDamage)
    {
        DamageUpPer += takeDamage;
    }

    public void SetFixedAttackStat(int damage)
    {
        DamageUpFixed += damage;
    }
    
    public void SetTrueDamage(int damage)
    {
        TrueDamageUp += damage;
    }

}