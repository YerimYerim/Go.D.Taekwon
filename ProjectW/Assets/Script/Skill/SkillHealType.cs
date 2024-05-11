using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class SkillHealType : SkillEffectBase, ISkillTargetHeal
{
    private int healValue;
    public override void DoSkill(List<GameActor> targetActor,  GameActor myActor)
    {
        DoHeal(targetActor, myActor);
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

    public void DoHeal(List<GameActor> targetActor,  GameActor myActor)
    {
        myActor.data.DoHeal(table.value_1 ??0);
        myActor.OnUpdateHp();
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.MinusAP(1);
        }
    }
}
