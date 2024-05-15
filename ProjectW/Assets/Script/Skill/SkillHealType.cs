using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class SkillHealType : SkillEffectBase, ISkillTargetHeal
{
    private int healValue;
    public override void DoSkill(List<GameActor> targetActor,  GameActor myActor)
    {
        DoHeal(myActor);
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

    public void DoHeal(GameActor targetActor)
    {
        targetActor.data.DoHeal(table.value_1 ??0);
        targetActor.OnUpdateHp();
    }

    public void DoHeal(List<GameActor> targetActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.DoHeal(table.value_1 ??0);
            targetActor[i].OnUpdateHp();
        }
    }
}
