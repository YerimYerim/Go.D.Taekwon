public record AttackStat
{
    public int damageUpInt;
    
    public AttackStat(int takeDamage)
    {
        this.damageUpInt = 0;
    }
    
    public void SetAttackStat(int takeDamage)
    {
        this.damageUpInt += takeDamage;
    }
    public void AddAttackStat(int damage)
    {
        damageUpInt += damage;
    }

    public void MinusAttackStat(int damage)
    {
        damageUpInt -= damage;
    }
}