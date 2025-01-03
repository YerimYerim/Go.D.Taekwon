public class UIAPGaugeIconMonster : UIAPGaugeIconBase
{
    private ActorEnemyData enemyData;
    
    public override int GetRemainSpellAP()
    {
        return enemyData.GetRemainAp();
    }

    public override int GetResetAp()
    {
        return enemyData.GetMaxAp();
    }

    public void SetData(GameActor data)
    {
        enemyData = data.data as ActorEnemyData;
        SetImage(data.resourceData.actor_portrait_small);
    }
}
