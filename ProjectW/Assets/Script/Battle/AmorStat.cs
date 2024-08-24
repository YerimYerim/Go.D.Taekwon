public record AmorStat
{
    public int amor = 0; // 방어도
    public int amorBreakInt;
    public float amorBreakPercent;
    public float takedamage;

    public int GetFinalDamage(int attackDamage)
    {
        return (int) (attackDamage * (1.0f - takedamage)) - (int) (amor * (1.0f - amorBreakPercent) - amorBreakInt);
    }

    public void AddAmor(int amorValue)
    {
        // amor 가 음수가 되지 않도록 보호
        if (amor + amorValue < 0)
        {
            amor = 0;
        }
        else
        {
            amor += amorValue;
        }
    }
}