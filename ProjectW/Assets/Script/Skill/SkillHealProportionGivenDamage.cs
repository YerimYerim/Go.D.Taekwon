using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHealProportionGivenDamage : SkillEffectBase, ISkillTurnSkill, ISkillTargetHeal, ISkillOnGiveDamage
{
    private GameActor _myActor;
    
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        _myActor = myActor;
        _myActor.data.OnGiveDamage += OnGiveDamage;
        foreach (var actor in targetActor)
        {
            actor.data.AddTurnSkill(this);
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data.effect_id ?? 0;
        remainTurn = data?.remain_turn_count ??0;
    }

    public override int GetValue()
    {
        return table.value_1 ?? 0;
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
        _myActor.data.OnGiveDamage -= OnGiveDamage;
        _myActor = null;
    }

    public void DoHeal(List<GameActor> targetActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.DoHeal(table.value_1 ?? 0);
            targetActor[i].OnUpdateHp(targetActor[i].data);
        }
    }
    public void DoHeal(List<GameActor> targetActor, int damage)
    {
    }

    public void OnGiveDamage(int damage)
    {
        _myActor.data.DoHeal(Mathf.CeilToInt((table.value_1 ?? 0) * 0.01f * damage));
        _myActor.OnUpdateHp(_myActor.data);
    }
}
