using System.Collections.Generic;

public class SkillIgnoreDamage : SkillEffectBase, ISkillIgnoreDamage, ISkillTurnSkill
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

    public void SetIgnoreDamage(GameActor target)
    {
        target.data.SetIgnoreDamage(true);
    }

    public void RemoveIgnoreDamage(GameActor target)
    {
        target.data.SetIgnoreDamage(false);
    }
    
    public void DoTurnSkill(GameActor target)
    {
        --remainTurn;
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
