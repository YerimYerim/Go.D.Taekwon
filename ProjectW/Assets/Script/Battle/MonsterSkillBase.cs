using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase : SkillGroupTableData
{
    public int MaxAP { get; private set; }
    public int CurrentAP{ get; private set; }

    public MonsterSkillBase(int maxAP)
    {
        MaxAP = maxAP;
        CurrentAP = maxAP;
    }

    public void ReduceAP(int ap)
    {
        CurrentAP = Math.Clamp(CurrentAP - ap, 0, MaxAP);
        if (CurrentAP <= 0)
        {
            ActiveSkill();
            CurrentAP = MaxAP;
        }
    }

    public void ResetAp()
    {
        CurrentAP = MaxAP;
    }
    public void GainAP(int ap)
    {
        CurrentAP += ap;
    }

    private void ActiveSkill()
    {
       var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
       gameMode?.BattleHandler?.DoSkillEnemyTurn();
    }
    
}
