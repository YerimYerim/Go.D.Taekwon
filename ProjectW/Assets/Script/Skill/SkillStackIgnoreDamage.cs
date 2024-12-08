using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStackIgnoreDamage : SkillEffectBase, ISkillIgnoreDamage, ISkillStackable, ISkillTurnSkill
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.AddTurnSkill(this);
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
    }

    public override int GetValue()
    {
        return remainTurn;
    }
    
    // ISkillIgnoreDamage
    public void SetIgnoreDamage(GameActor target)
    {
        target.data.SetIgnoreDamage(true);
    }

    public void RemoveIgnoreDamage(GameActor target)
    {
        target.data.SetIgnoreDamage(false);
    }
    
    public void RemoveStack()
    {
        --remainTurn;
    }

    public void DoStackEffect(GameActor target)
    {
        
    }

    // ISkillTurnSkill
    public void DoTurnSkill(GameActor target)
    {

    }

    public int GetRemainTime()
    {
        return remainTurn;
    }

    public void DoTurnEndSkill(GameActor target)
    {
        RemoveIgnoreDamage(target);
    }
}
