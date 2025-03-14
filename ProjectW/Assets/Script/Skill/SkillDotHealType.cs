using System.Collections.Generic;

public class SkillDotHealType : SkillEffectBase, ISkillTurnSkill, ISkillTargetHeal
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; i++)
        {
            targetActor[i].data.AddTurnSkill(this);
        }  
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
        remainTurn = data?.remain_turn_count ?? 0;
    }

    public override int GetValue()
    {
        return table?.value_1 ?? 0;
    }

    public void DoTurnSkill(GameActor enemy)
    {
        var list = new List<GameActor> {enemy};
        DoHeal(list);
        --remainTurn;
    }

    public int GetRemainTime()
    {
        return remainTurn;
    }

    public void DoTurnEndSkill(GameActor target)
    {
        
    }

    public void DoHeal(List<GameActor> targetActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.DoHeal(GetValue());
        }
    }
}
