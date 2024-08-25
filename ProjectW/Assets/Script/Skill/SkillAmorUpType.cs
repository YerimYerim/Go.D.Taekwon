using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class SkillAmorUpType : SkillEffectBase, ISkillAmorUp
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            if (targetActor[i].data is ActorEnemyData)
            {
                ((ActorEnemyData)targetActor[i].data).AddAmorStat(table?.value_1 ?? 0);
                targetActor[i].OnAddDef(table?.value_1 ?? 0);
            }
            else if (targetActor[i].data is ActorPlayerData)
            {
                ((ActorPlayerData)targetActor[i].data).AddAmorStat(table?.value_1 ?? 0);
                targetActor[i].OnAddDef(table?.value_1 ?? 0);
            }
        }
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
