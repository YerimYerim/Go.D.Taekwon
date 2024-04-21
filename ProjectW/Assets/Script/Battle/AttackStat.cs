public record AttackStat
{
    public int skillDamage;
    public int damageUpInt;
    public float damageUpPercent;
    
    public int GetFinalAttackDamage(int attackDamage)
    {
        return (int)((skillDamage + damageUpInt) * (1.0f + damageUpPercent));
    }
}