using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAmorDownType : SkillEffectBase, ISkillAmorDown
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            ReduceAmor(targetActor[i]);
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data.effect_id ?? 0;
    }

    public override int GetValue()
    {
        return table.value_1 ?? 0;
    }
    
    public void ReduceAmor(GameActor targetActor)
    {
       targetActor.data.AddAmorStat(-GetValue());
    }

    public void AddDebuff()
    {
    }
    
    public void RemoveDebuff()
    {
    }
}
