using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillApUp : SkillEffectBase, ISkillTargetAPUp
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        AddAp(targetActor);
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

    public void AddAp(List<GameActor> enemyTarget)
    {
        for (int i = 0; i < enemyTarget.Count; ++i)
        {
            enemyTarget[i].data.AddAP(table?.value_1 ?? 0);
        }
    }
}
