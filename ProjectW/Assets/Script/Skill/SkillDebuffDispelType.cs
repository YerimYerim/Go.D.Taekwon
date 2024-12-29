using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDebuffDispelType : SkillEffectBase, ISkillDispelTurnSkill
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            DispelTurnSkill(targetActor[i]);
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
    }

    public override int GetValue()
    {
        return table.value_1 ?? 0;
    }

    public void DispelTurnSkill(GameActor targetActor)
    {
        var allDispel = targetActor.data.turnSkill.FindAll(_ => _.GetTable().effect_tag == EFFECT_TAG.EFFECT_TAG_DEBUFF);
        for (var i = 0; i < allDispel.Count; i++)
        {
            if (i < GetValue())
            {
                targetActor.data.turnSkill.Remove(allDispel[i]);
            }
        }
    }
}
