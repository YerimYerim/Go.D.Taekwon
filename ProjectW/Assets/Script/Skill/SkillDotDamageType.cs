using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDotDamageType : SkillEffectBase, ISkillTargetDamage, ISkillTargetDebuff
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.AddDebuff(this);
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
        remainTurn = data?.remain_turn_count ??0;
    }

    public override int GetValue()
    {
        return table.value_1 ?? 0;
    }
    
    public void DoDebuff(GameActor enemy)
    {
        DoDamage(enemy);
        --remainTurn;
    }

    public void DoDamage(GameActor enemy)
    {
        enemy.data.DoDamaged(GetValue());
    }
}
