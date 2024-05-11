using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAmorUpType : SkillEffectBase, ISkillAmorUp
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        myActor.data.AddAP( GetValue());   
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
    }

    public override int GetValue()
    {
        return table?.value_1 ?? 0;
    }

    public void AddAmor(GameActor target)
    {
        target.data.AddAmorStat(table.value_1??0);
    }
}
