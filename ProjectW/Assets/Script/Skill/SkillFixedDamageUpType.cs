using System.Collections.Generic;

public class SkillFixedDamageUpType :SkillEffectBase, ISkillTurnSkill, ISkilDamageUp
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
    

    public void DoTurnSkill(GameActor enemy)
    {
        --remainTurn;
    }

    public void DoDamageUp(GameActor target)
    {
        target.data.SetFixedDamageStat(GetValue());
    }

    public void DoTurnEndSkill(GameActor target)
    {
        target.data.SetFixedDamageStat(-GetValue());
    }

    public int GetRemainTime()
    {
        return remainTurn;
    }

}
