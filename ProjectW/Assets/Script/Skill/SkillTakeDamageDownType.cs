using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTakeDamageDownType : SkillEffectBase, ISkillTakeDamageUp, ISkillTurnSkill
{
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

    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.AddTurnSkill(this);
        }
    }
    
    public void DoTakeDamageUp(GameActor target)
    {
        target.data.SetTakeDamageStat(-GetValue());
    }

    public void DoTurnSkill(GameActor enemy)
    {
        --remainTurn;
    }

    public void DoTurnEndSkill(GameActor target)
    {   
        target.data.SetTakeDamageStat(GetValue());
    }

    public int GetRemainTime()
    {
        return remainTurn;
    }
}
