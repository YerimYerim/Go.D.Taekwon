using System.Collections.Generic;

public class SkillDotHealType : SkillEffectBase, ISkillTargetBuff, ISkillTargetHeal
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; i++)
        {
            targetActor[i].data.AddBuff(this);
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

    public void DoBuff(GameActor enemy)
    {
        DoHeal(new List<GameActor> {enemy});
        --remainTurn;
    }
    public void DoHeal(List<GameActor> targetActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.DoHeal(GetValue());
        }
    }
}
