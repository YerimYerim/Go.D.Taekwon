using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase : SkillGroupTableData
{
    public int MaxAP { get; private set; }
    public int CurrentAP{ get; private set; }

    public event Action OnEventAPZero;
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
        }
    }
    public void GainAP(int ap)
    {
        CurrentAP = Math.Clamp(CurrentAP - ap, 0, MaxAP);
    }

    private void ActiveSkill()
    {
        OnEventAPZero?.Invoke();
    }
    
}
