public record HealStat
{
    private int heal;
    private float healPercent;

    public int GetFinalHeal(int maxHp)
    {
        return (int)(heal * (1.0f + healPercent));
    }
}