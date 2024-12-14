using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHealProportionTakeDamage : SkillEffectBase, ISkillTurnSkill, ISkillTargetHeal, ISkillOnTakenDamage
{
    private GameActor _myActor;
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        _myActor = myActor;
        _myActor.data.OnGiveDamage += OnTakenDamage;
        myActor.data.AddTurnSkill(this);
    }

    public override void InitSkillType(SpellEffectTableData data)
    {

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
        _myActor.data.OnGiveDamage -= OnTakenDamage;
        _myActor = null;
    }

    public void DoHeal(List<GameActor> targetActor)
    {
      
    }

    public void OnTakenDamage(int damage)
    {
        _myActor.data.DoHeal(Mathf.CeilToInt((table.value_1 ?? 0) * 0.01f * damage));
        _myActor.OnUpdateHp(_myActor.data);
    }
}
