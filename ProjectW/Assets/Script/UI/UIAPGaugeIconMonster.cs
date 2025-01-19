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
        if (data == null)
        {
            gameObject.SetActive(false);
            IsShow = false;
        }
        else
        {
            enemyData = data.data as ActorEnemyData;
            IsShow = true;
            SetImage(data.resourceData.actor_portrait_small);
        }
    }
}
