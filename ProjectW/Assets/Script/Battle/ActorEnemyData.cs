using System.Collections.Generic;
using UnityEngine;

public class ActorEnemyData : ActorDataBase
{
    public int ActionPoint { get; private set; }
    private List<MonsterSkillBase> _skillBases;
    
    public void InitAP(int AP)
    {
        ActionPoint = AP;
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
    public void AddAP(int addAp)
    {
        ActionPoint += addAp;
    }
}
